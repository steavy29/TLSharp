﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ionic.Zlib;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.MTProto.Crypto;
using Telegram.Net.Core.Requests;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Network
{
    public class MtProtoSender
    {
        private readonly TcpTransport transport;
        private readonly Session session;

        private readonly Dictionary<long, Tuple<MTProtoRequest, TaskCompletionSource<bool>>> runningRequests = new Dictionary<long, Tuple<MTProtoRequest, TaskCompletionSource<bool>>>();
        private readonly List<long> needConfirmation = new List<long>();

        private TaskCompletionSource<bool> finishedListening;
        public Task finishedListeningTask => finishedListening.Task;

        public event EventHandler<Updates> UpdateMessage;
        private event EventHandler Invalidated; // todo

        public MtProtoSender(TcpTransport transport, Session session)
        {
            this.transport = transport;
            this.session = session;

            StartListening();
        }

        private async void StartListening()
        {
            finishedListening = new TaskCompletionSource<bool>();
            while (true)
            {
                var message = await transport.Receieve().ConfigureAwait(false);
                if (message == null)
                    break;

                var decodedMessage = DecodeMessage(message.body);

                using (var messageStream = new MemoryStream(decodedMessage.Item1, false))
                using (var messageReader = new BinaryReader(messageStream))
                {
                    ProcessMessage(decodedMessage.Item2, decodedMessage.Item3, messageReader);
                }
            }
            finishedListening.SetResult(true);
        }

        public async Task Send(MTProtoRequest request)
        {
            if (needConfirmation.Any()) // TODO: move to separate task-thread
            {
                var ackRequest = new AckRequestLong(needConfirmation);
                using (var memory = new MemoryStream())
                using (var writer = new BinaryWriter(memory))
                {
                    ackRequest.MessageId = session.GetNewMessageId();

                    ackRequest.OnSend(writer);
                    await Send(memory.ToArray(), ackRequest);
                    needConfirmation.Clear();
                }
            }

            TaskCompletionSource<bool> responseSource;
            using (var memory = new MemoryStream())
            using (var writer = new BinaryWriter(memory))
            {
                var messageId = session.GetNewMessageId();
                request.MessageId = messageId;
                Debug.WriteLine($"Send request - {messageId}");

                responseSource = new TaskCompletionSource<bool>();
                runningRequests.Add(request.MessageId, Tuple.Create(request, responseSource));

                request.OnSend(writer);
                await Send(memory.ToArray(), request);
            }

            await responseSource.Task;
            runningRequests.Remove(request.MessageId);
        }

        private async Task Send(byte[] packet, MTProtoRequest request)
        {
            byte[] msgKey;
            byte[] ciphertext;
            using (MemoryStream plaintextPacket = MakeMemory(8 + 8 + 8 + 4 + 4 + packet.Length))
            {
                using (BinaryWriter plaintextWriter = new BinaryWriter(plaintextPacket))
                {
                    plaintextWriter.Write(session.salt);
                    plaintextWriter.Write(session.id);
                    plaintextWriter.Write(request.MessageId);
                    plaintextWriter.Write(GenerateSequence(request.Confirmed));
                    plaintextWriter.Write(packet.Length);
                    plaintextWriter.Write(packet);

                    msgKey = Helpers.CalcMsgKey(plaintextPacket.GetBuffer());
                    ciphertext = AES.EncryptAES(Helpers.CalcKey(session.authKey.Data, msgKey, true), plaintextPacket.GetBuffer());
                }
            }

            using (MemoryStream ciphertextPacket = MakeMemory(8 + 16 + ciphertext.Length))
            {
                using (BinaryWriter writer = new BinaryWriter(ciphertextPacket))
                {
                    writer.Write(session.authKey.Id);
                    writer.Write(msgKey);
                    writer.Write(ciphertext);

                    await transport.Send(ciphertextPacket.GetBuffer());
                }
            }
        }

        private void ProcessMessage(long messageId, int sequence, BinaryReader messageReader)
        {
            // TODO: check salt
            // TODO: check sessionid
            // TODO: check seqno

            //logger.debug("processMessage: msg_id {0}, sequence {1}, data {2}", BitConverter.ToString(((MemoryStream)messageReader.BaseStream).GetBuffer(), (int) messageReader.BaseStream.Position, (int) (messageReader.BaseStream.Length - messageReader.BaseStream.Position)).Replace("-","").ToLower());
            needConfirmation.Add(messageId);

            uint code = messageReader.ReadUInt32();
            switch (code)
            {
                case 0x73f1f8dc: // messages container
                                 //logger.debug("MSG container");
                    HandleContainer(messageId, sequence, messageReader);
                    break;
                case 0x7abe77ec: // ping
                                 //logger.debug("MSG ping");
                    break;
                case 0x347773c5: // pong
                                 //logger.debug("MSG pong");
                    break;
                case 0xae500895: // future_salts
                                 //logger.debug("MSG future_salts");
                    break;
                case 0x9ec20908: // new_session_created
                                 //logger.debug("MSG new_session_created");
                    HandleNewSessionCreated(messageReader);
                    break;
                case 0x62d6b459: // msgs_ack
                                 //logger.debug("MSG msds_ack");
                    break;
                case 0xedab447b: // bad_server_salt
                                 //logger.debug("MSG bad_server_salt");
                    HandleBadServerSalt(messageId, sequence, messageReader);
                    break;
                case 0xa7eff811: // bad_msg_notification
                                 //logger.debug("MSG bad_msg_notification");
                    HandleBadMsgNotification(messageId, sequence, messageReader);
                    break;
                case 0x276d3ec6: // msg_detailed_info
                                 //logger.debug("MSG msg_detailed_info");
                    break;
                case 0xf35c6d01: // rpc_result
                                 //logger.debug("MSG rpc_result");
                    HandleRpcResult(messageReader);
                    break;
                case 0x3072cfa1: // gzip_packed
                                 //logger.debug("MSG gzip_packed");
                    HandleGzipPacked(messageId, sequence, messageReader);
                    break;
                case 0xe317af7e: // updatesTooLong
                case 0xd3f45784: // updateShortMessage
                case 0x2b2fbd4e: // updateShortChatMessage
                case 0x78d4dec1: // updateShort
                case 0x725b04c3: // updatesCombined
                case 0x74ae4240: // updates
                    {
                        HandleUpdateMessage(messageReader, code);
                        break;
                    }
                default:
                    Debug.WriteLine($"Unknown error: {code}");
                    break;
            }
        }

        private Tuple<byte[], long, int> DecodeMessage(byte[] body)
        {
            byte[] message;
            long remoteMessageId;
            int remoteSequence;

            using (var inputStream = new MemoryStream(body))
            using (var inputReader = new BinaryReader(inputStream))
            {
                if (inputReader.BaseStream.Length < 8)
                    throw new InvalidOperationException("Can't decode packet");

                long remoteAuthKeyId = inputReader.ReadInt64(); // TODO: check auth key id
                byte[] msgKey = inputReader.ReadBytes(16); // TODO: check msg_key correctness
                AESKeyData keyData = Helpers.CalcKey(session.authKey.Data, msgKey, false);

                byte[] plaintext = AES.DecryptAES(keyData, inputReader.ReadBytes((int)(inputStream.Length - inputStream.Position)));

                using (MemoryStream plaintextStream = new MemoryStream(plaintext))
                using (BinaryReader plaintextReader = new BinaryReader(plaintextStream))
                {
                    plaintextReader.ReadUInt64(); // remoteSalt
                    plaintextReader.ReadUInt64(); // remoteSessionId
                    remoteMessageId = plaintextReader.ReadInt64();
                    remoteSequence = plaintextReader.ReadInt32();
                    int msgLen = plaintextReader.ReadInt32();
                    message = plaintextReader.ReadBytes(msgLen);
                }
            }
            return new Tuple<byte[], long, int>(message, remoteMessageId, remoteSequence);
        }

        private int GenerateSequence(bool confirmed)
        {
            return confirmed ? session.sequence++ * 2 + 1 : session.sequence * 2;
        }

        private void OnUpdateMessage(Updates update)
        {
            UpdateMessage?.Invoke(this, update);
        }

        private void OnInvalidated()
        {
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        public static MemoryStream MakeMemory(int len)
        {
            return new MemoryStream(new byte[len], 0, len, true, true);
        }

        #region Message Handlers

        private void HandleRpcResult(BinaryReader messageReader)
        {
            long requestId = messageReader.ReadInt64();
            Debug.WriteLine($"HandleRpcResult: requestId - {requestId}");

            if (!runningRequests.ContainsKey(requestId))
            {
                return;
            }
            var requestInfo = runningRequests[requestId];
            MTProtoRequest request = requestInfo.Item1;

            request.ConfirmReceived = true;

            uint innerCode = messageReader.ReadUInt32();
            if (innerCode == 0x2144ca19) // rpc_error
            {
                int errorCode = messageReader.ReadInt32();
                string errorMessage = Serializers.String.Read(messageReader);
                request.OnError(errorCode, errorMessage);
                requestInfo.Item2.SetResult(true);
            }
            else if (innerCode == 0x3072cfa1) // gzip_packed
            {
                try
                {
                    byte[] packedData = Serializers.Bytes.Read(messageReader);
                    using (var ms = new MemoryStream())
                    {
                        using (var packedStream = new MemoryStream(packedData, false))
                        using (var zipStream = new GZipStream(packedStream, CompressionMode.Decompress))
                        {
                            zipStream.CopyTo(ms);
                            ms.Position = 0;
                        }
                        using (var compressedReader = new BinaryReader(ms))
                        {
                            request.OnResponse(compressedReader);
                            requestInfo.Item2.SetResult(true);
                        }
                    }
                }
                catch (ZlibException)
                {
                }
            }
            else
            {
                messageReader.BaseStream.Position -= 4;
                request.OnResponse(messageReader);

                requestInfo.Item2.SetResult(true);
            }
        }

        private void HandleContainer(long messageId, int sequence, BinaryReader messageReader)
        {
            int size = messageReader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                long innerMessageId = messageReader.ReadInt64(); // TODO: Remove this reading and call ProcessMessage directly(remove appropriate params in ProcMsg)
                Debug.WriteLine($"Container innerMessageId: {innerMessageId}");
                messageReader.ReadInt32(); // innerSequence
                int innerLength = messageReader.ReadInt32();
                long beginPosition = messageReader.BaseStream.Position;

                ProcessMessage(innerMessageId, sequence, messageReader);
                messageReader.BaseStream.Position = beginPosition + innerLength; // shift to next message
            }
        }

        private void HandleBadServerSalt(long messageId, int sequence, BinaryReader messageReader)
        {
            long badMsgId = messageReader.ReadInt64();
            int badMsgSeqNo = messageReader.ReadInt32();
            int errorCode = messageReader.ReadInt32();
            ulong newSalt = messageReader.ReadUInt64();

            session.salt = newSalt;

            if (!runningRequests.ContainsKey(badMsgId))
                return;

            runningRequests[badMsgId].Item1.OnError(errorCode, null);
            runningRequests[badMsgId].Item2.SetResult(true);
        }

        private void HandleBadMsgNotification(long messageId, int sequence, BinaryReader messageReader)
        {
            long badRequestId = messageReader.ReadInt64();
            messageReader.ReadInt32(); // badRequestSequence
            int errorCode = messageReader.ReadInt32();

            if (runningRequests.ContainsKey(badRequestId))
            {
                runningRequests[badRequestId].Item1.OnError(errorCode, null);
                runningRequests[badRequestId].Item2.SetResult(true);
            }
        }

        private void HandleGzipPacked(long messageId, int sequence, BinaryReader messageReader)
        {
            byte[] packedData = GZipStream.UncompressBuffer(Serializers.Bytes.Read(messageReader));
            using (MemoryStream packedStream = new MemoryStream(packedData, false))
            using (BinaryReader compressedReader = new BinaryReader(packedStream))
            {
                ProcessMessage(messageId, sequence, compressedReader);
            }
        }

        private void HandleUpdateMessage(BinaryReader messageReader, uint updateDataCode)
        {
            var update = TLObject.Read<Updates>(messageReader, updateDataCode);
            OnUpdateMessage(update);
        }

        private void HandleNewSessionCreated(BinaryReader messageReader)
        {
            var firstMsgId = messageReader.ReadUInt64();
            var uniqueId = messageReader.ReadUInt64();
            var serverSalt = messageReader.ReadUInt64();

            session.salt = serverSalt;
            session.id = uniqueId;
        }

        #endregion
    }
}