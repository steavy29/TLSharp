using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zlib;
using TLSharp.Core.MTProto;
using TLSharp.Core.MTProto.Crypto;
using TLSharp.Core.Requests;
using TLSharp.Core.Utils;

namespace TLSharp.Core.Network
{
    public class MtProtoSender
    {
        private readonly TcpTransport _transport;
        private readonly Session _session;

        private readonly Dictionary<long, Tuple<MTProtoRequest, TaskCompletionSource<bool>>> _runningRequests = new Dictionary<long, Tuple<MTProtoRequest, TaskCompletionSource<bool>>>();
        private readonly List<long> _needConfirmation = new List<long>();

        private TaskCompletionSource<bool> _finishedListening;
        public Task FinishedListeningTask => _finishedListening.Task;

        public event EventHandler<Updates> UpdateMessage; 

        public MtProtoSender(TcpTransport transport, Session session)
        {
            _transport = transport;
            _session = session;

            StartListening();
        }

        private async void StartListening()
        {
            _finishedListening = new TaskCompletionSource<bool>();
            while (true)
            {
                var message = await _transport.ReceieveFixed().ConfigureAwait(false);
                if (message == null)
                    break;

                var decodedMessage = DecodeMessage(message.Body);
                
                using (var messageStream = new MemoryStream(decodedMessage.Item1, false))
                using (var messageReader = new BinaryReader(messageStream))
                {
                    ProcessMessage(decodedMessage.Item2, decodedMessage.Item3, messageReader);
                }
            }
            _finishedListening.SetResult(true);
        }

        public async Task Send(MTProtoRequest request)
        {
            if (_needConfirmation.Any()) // TODO: move to separate task-thread
            {
                var ackRequest = new AckRequestLong(_needConfirmation);
                using (var memory = new MemoryStream())
                using (var writer = new BinaryWriter(memory))
                {
                    ackRequest.MessageId = _session.GetNewMessageId();

                    ackRequest.OnSend(writer);
                    await Send(memory.ToArray(), ackRequest);
                    _needConfirmation.Clear();
                }
            }

            TaskCompletionSource<bool> responseSource;
            using (var memory = new MemoryStream())
            using (var writer = new BinaryWriter(memory))
            {
                var messageId = _session.GetNewMessageId();
                request.MessageId = messageId;
                Debug.WriteLine($"Send request - {messageId}");
                
                if (!_runningRequests.ContainsKey(request.MessageId))
                {
                    responseSource = new TaskCompletionSource<bool>();
                    _runningRequests.Add(request.MessageId, Tuple.Create(request, responseSource));
                }
                else
                {
                    // when resending the request
                    responseSource = _runningRequests[request.MessageId].Item2;
                }

                request.OnSend(writer);
                await Send(memory.ToArray(), request);
            }
            
            await responseSource.Task;
            _runningRequests.Remove(request.MessageId);

            if (request.NeedResend)
            {
                await Send(request);
            }
            else
            {
                _session.Save();
            }
        }

        private async Task Send(byte[] packet, MTProtoRequest request)
        {
            byte[] msgKey;
            byte[] ciphertext;
            using (MemoryStream plaintextPacket = MakeMemory(8 + 8 + 8 + 4 + 4 + packet.Length))
            {
                using (BinaryWriter plaintextWriter = new BinaryWriter(plaintextPacket))
                {
                    plaintextWriter.Write(_session.Salt);
                    plaintextWriter.Write(_session.Id);
                    plaintextWriter.Write(request.MessageId);
                    plaintextWriter.Write(GenerateSequence(request.Confirmed));
                    plaintextWriter.Write(packet.Length);
                    plaintextWriter.Write(packet);

                    msgKey = Helpers.CalcMsgKey(plaintextPacket.GetBuffer());
                    ciphertext = AES.EncryptAES(Helpers.CalcKey(_session.AuthKey.Data, msgKey, true), plaintextPacket.GetBuffer());
                }
            }

            using (MemoryStream ciphertextPacket = MakeMemory(8 + 16 + ciphertext.Length))
            {
                using (BinaryWriter writer = new BinaryWriter(ciphertextPacket))
                {
                    writer.Write(_session.AuthKey.Id);
                    writer.Write(msgKey);
                    writer.Write(ciphertext);

                    await _transport.Send(ciphertextPacket.GetBuffer());
                }
            }
        }

        private bool ProcessMessage(long messageId, int sequence, BinaryReader messageReader)
        {
            // TODO: check salt
            // TODO: check sessionid
            // TODO: check seqno

            //logger.debug("processMessage: msg_id {0}, sequence {1}, data {2}", BitConverter.ToString(((MemoryStream)messageReader.BaseStream).GetBuffer(), (int) messageReader.BaseStream.Position, (int) (messageReader.BaseStream.Length - messageReader.BaseStream.Position)).Replace("-","").ToLower());
            _needConfirmation.Add(messageId);

            uint code = messageReader.ReadUInt32();
            //messageReader.BaseStream.Position -= 4; // Whe need to rewind back?
            switch (code)
            {
                case 0x73f1f8dc: // messages container
                                 //logger.debug("MSG container");
                    return HandleContainer(messageId, sequence, messageReader);
                case 0x7abe77ec: // ping
                                 //logger.debug("MSG ping");
                    return false;
                case 0x347773c5: // pong
                                 //logger.debug("MSG pong");
                    return false;
                case 0xae500895: // future_salts
                                 //logger.debug("MSG future_salts");
                    return false;
                case 0x9ec20908: // new_session_created
                                 //logger.debug("MSG new_session_created");
                    return false;
                case 0x62d6b459: // msgs_ack
                                 //logger.debug("MSG msds_ack");
                    return false;
                case 0xedab447b: // bad_server_salt
                                 //logger.debug("MSG bad_server_salt");
                    return HandleBadServerSalt(messageId, sequence, messageReader);
                case 0xa7eff811: // bad_msg_notification
                                 //logger.debug("MSG bad_msg_notification");
                    return HandleBadMsgNotification(messageId, sequence, messageReader);
                case 0x276d3ec6: // msg_detailed_info
                                 //logger.debug("MSG msg_detailed_info");
                    return false;
                case 0xf35c6d01: // rpc_result
                                 //logger.debug("MSG rpc_result");
                    return HandleRpcResult(messageReader);
                case 0x3072cfa1: // gzip_packed
                                 //logger.debug("MSG gzip_packed");
                    return HandleGzipPacked(messageId, sequence, messageReader);
                case 0xe317af7e: // updatesTooLong
                case 0xd3f45784: // updateShortMessage
                case 0x2b2fbd4e: // updateShortChatMessage
                case 0x78d4dec1: // updateShort
                case 0x725b04c3: // updatesCombined
                case 0x74ae4240: // updates
                {
                    if (code == 0xe317af7e)
                    {
                        
                    }
                    Debug.WriteLine($"Update message: {code}");
                    HandleUpdateMessage(messageReader, code);
                    return false;
                }
                default:
                    //logger.debug("unknown message: {0}", code);
                    return false;
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
                AESKeyData keyData = Helpers.CalcKey(_session.AuthKey.Data, msgKey, false);

                byte[] plaintext = AES.DecryptAES(keyData, inputReader.ReadBytes((int)(inputStream.Length - inputStream.Position)));

                using (MemoryStream plaintextStream = new MemoryStream(plaintext))
                using (BinaryReader plaintextReader = new BinaryReader(plaintextStream))
                {
                    var remoteSalt = plaintextReader.ReadUInt64();
                    var remoteSessionId = plaintextReader.ReadUInt64();
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
            return confirmed ? _session.Sequence++ * 2 + 1 : _session.Sequence * 2;
        }

        private MemoryStream MakeMemory(int len)
        {
            return new MemoryStream(new byte[len], 0, len, true, true);
        }

        #region Message Handlers
        
        private bool HandleRpcResult(BinaryReader messageReader)
        {
            long requestId = messageReader.ReadInt64();
            Debug.WriteLine($"HandleRpcResult: requestId - {requestId}");

            if (!_runningRequests.ContainsKey(requestId))
            {
                return false;
            }
            var requestInfo = _runningRequests[requestId];
            MTProtoRequest request = requestInfo.Item1;

            request.ConfirmReceived = true;
            
            uint innerCode = messageReader.ReadUInt32();
            if (innerCode == 0x2144ca19)
            { // rpc_error
                int errorCode = messageReader.ReadInt32();
                string errorMessage = Serializers.String.read(messageReader);
                request.OnError(errorCode, errorMessage);
                requestInfo.Item2.SetResult(true);

                if (errorMessage.StartsWith("FLOOD_WAIT_"))
                {
                    var resultString = Regex.Match(errorMessage, @"\d+").Value;
                    var seconds = int.Parse(resultString);
                    Debug.WriteLine($"Should wait {seconds} sec.");
                    Thread.Sleep(1000 * seconds);
                }
                else if (errorMessage.StartsWith("PHONE_MIGRATE_"))
                {
                    var resultString = Regex.Match(errorMessage, @"\d+").Value;
                    var dcIdx = int.Parse(resultString);
                    var exception = new InvalidOperationException($"Your phone number registered to {dcIdx} dc. Please update settings. See https://github.com/sochix/TLSharp#i-get-an-error-migrate_x for details.");
                    exception.Data.Add("dcId", dcIdx);
                    throw exception;
                }
                else
                {
                    throw new InvalidOperationException(errorMessage);
                }

            }
            else if (innerCode == 0x3072cfa1)
            {
                try
                {
                    // gzip_packed
                    byte[] packedData = Serializers.Bytes.read(messageReader);
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
                            requestInfo.Item2.SetResult(true); //////////////////////////////////////////////////////////////////////
                        }
                    }
                }
                catch (ZlibException ex)
                {

                }
            }
            else
            {
                messageReader.BaseStream.Position -= 4;
                request.OnResponse(messageReader);

                requestInfo.Item2.SetResult(true); //////////////////////////////////////////////////////////////////////
            }

            return false;
        }

        private bool HandleContainer(long messageId, int sequence, BinaryReader messageReader)
        {
            int size = messageReader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                long innerMessageId = messageReader.ReadInt64(); // TODO: Remove this reading and call ProcessMessage directly(remove appropriate params in ProcMsg)
                Debug.WriteLine($"Container innerMessageId: {innerMessageId}");
                int innerSequence = messageReader.ReadInt32();
                int innerLength = messageReader.ReadInt32();
                long beginPosition = messageReader.BaseStream.Position;
                try
                {
                    if (!ProcessMessage(innerMessageId, sequence, messageReader))
                    {
                        messageReader.BaseStream.Position = beginPosition + innerLength;
                    }
                }
                catch (Exception e)
                {
                    //	logger.error("failed to process message in contailer: {0}", e);
                    messageReader.BaseStream.Position = beginPosition + innerLength;
                }
            }

            return false;
        }

        private bool HandleBadServerSalt(long messageId, int sequence, BinaryReader messageReader)
        {
            long badMsgId = messageReader.ReadInt64();
            int badMsgSeqNo = messageReader.ReadInt32();
            int errorCode = messageReader.ReadInt32();
            ulong newSalt = messageReader.ReadUInt64();

            _session.Salt = newSalt;

            if (!_runningRequests.ContainsKey(badMsgId))
                return true;

            _runningRequests[badMsgId].Item2.SetResult(false);

            //logger.debug("bad_server_salt: msgid {0}, seq {1}, errorcode {2}, newsalt {3}", badMsgId, badMsgSeqNo, errorCode, newSalt);

            //resend
            // TODO: should we blindly resend the request or propagate error and let Session handle this with some retry-limit logic?
            //Send(request);

            return true;
        }

        private bool HandleBadMsgNotification(long messageId, int sequence, BinaryReader messageReader)
        {
            long badRequestId = messageReader.ReadInt64();
            int badRequestSequence = messageReader.ReadInt32();
            int errorCode = messageReader.ReadInt32();

            string message;
            switch (errorCode)
            {
                case 16:
                    message = "msg_id too low (most likely, client time is wrong; it would be worthwhile to synchronize it using msg_id notifications and re-send the original message with the “correct” msg_id or wrap it in a container with a new msg_id if the original message had waited too long on the client to be transmitted)";
                    break;
                case 17:
                    message = "msg_id too high (similar to the previous case, the client time has to be synchronized, and the message re-sent with the correct msg_id)";
                    break;
                case 18:
                    message = "incorrect two lower order msg_id bits (the server expects client message msg_id to be divisible by 4)";
                    break;
                case 19:
                    message = "container msg_id is the same as msg_id of a previously received message (this must never happen)";
                    break;
                case 20:
                    message = "message too old, and it cannot be verified whether the server has received a message with this msg_id or not";
                    break;
                case 32:
                    message = "msg_seqno too low (the server has already received a message with a lower msg_id but with either a higher or an equal and odd seqno)";
                    break;
                case 33:
                    message = " msg_seqno too high (similarly, there is a message with a higher msg_id but with either a lower or an equal and odd seqno)";
                    break;
                case 34:
                    message = "an even msg_seqno expected (irrelevant message), but odd received";
                    break;
                case 35:
                    message = "odd msg_seqno expected (relevant message), but even received";
                    break;
                case 48:
                    message = "incorrect server salt (in this case, the bad_server_salt response is received with the correct salt, and the message is to be re-sent with it)";
                    break;
                case 64:
                    message = "invalid container";
                    break;
                default:
                    message = "unknown error";
                    break;
            }


            if (_runningRequests.ContainsKey(badRequestId))
            {
                _runningRequests[badRequestId].Item1.OnError(errorCode, message);
                _runningRequests[badRequestId].Item2.SetResult(false);
            }
            
            /*
			logger.debug("bad_msg_notification: msgid {0}, seq {1}, errorcode {2}", requestId, requestSequence,
						 errorCode);
			*/
            /*
			if (!runningRequests.ContainsKey(requestId))
			{
				logger.debug("bad msg notification on unknown request");
				return true;
			}
			*/

            //OnBrokenSessionEvent();
            //MTProtoRequest request = runningRequests[requestId];
            //request.OnException(new MTProtoBadMessageException(errorCode));

            return true;
        }

        private bool HandleGzipPacked(long messageId, int sequence, BinaryReader messageReader)
        {
            uint code = messageReader.ReadUInt32();
            byte[] packedData = GZipStream.UncompressBuffer(Serializers.Bytes.read(messageReader));
            using (MemoryStream packedStream = new MemoryStream(packedData, false))
            using (BinaryReader compressedReader = new BinaryReader(packedStream))
            {
                ProcessMessage(messageId, sequence, compressedReader);
            }

            return true;
        }

        private bool HandleUpdateMessage(BinaryReader messageReader, uint updateDataCode)
        {
            var update = TL.Parse<Updates>(messageReader, updateDataCode);
            OnUpdateMessage(update);
            return true;
        }

        #endregion

        protected virtual void OnUpdateMessage(Updates update)
        {
            UpdateMessage?.Invoke(this, update);
        }
    }
}
