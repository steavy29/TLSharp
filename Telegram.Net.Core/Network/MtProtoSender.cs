using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zlib;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.MTProto.Crypto;
using Telegram.Net.Core.Requests;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Network
{
    public class MtProtoSender : IDisposable
    {
        private static int pingDelayMs = 60000;
        private static int pingTimeoutMs = 10000;

        private bool isClosed;
        private Exception exceptionForClosedConnection => new Exception("Channel was closed.");

        private readonly TcpTransport transport;
        private readonly Session session;

        private readonly Dictionary<long, Tuple<MtProtoRequest, TaskCompletionSource<bool>>> runningRequests = new Dictionary<long, Tuple<MtProtoRequest, TaskCompletionSource<bool>>>();
        private List<long> needConfirmation = new List<long>();

        private TaskCompletionSource<bool> finishedListening;
        public Task finishedListeningTask => finishedListening.Task;

        public readonly string dcServerAddress;

        public event EventHandler<Updates> UpdateMessage;
        public event EventHandler Broken;

        public MtProtoSender(Session session, bool immediateStart = false)
        {
            dcServerAddress = session.serverAddress;

            this.session = session;

            Debug.WriteLine($"Connecting to {session.serverAddress}:{session.port}..");
            transport = new TcpTransport(session.serverAddress, session.port);
            Debug.WriteLine($"Successfully connected to {session.serverAddress}:{session.port}");

            if (immediateStart)
            {
                Start();
            }
        }

        public void Start()
        {
            StartListening();
            StartPingLoop();
        }

        private async void StartPingLoop()
        {
            await Task.Delay(pingDelayMs);
            while (!isClosed)
            {
                try
                {
                    var pingRequest = new PingRequest();
                    var pingSendTask = Send(pingRequest).ContinueWith(t => { var ignored = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

                    var pingTimeoutTask = Task.Delay(pingTimeoutMs);
                    if (await Task.WhenAny(pingTimeoutTask, pingSendTask) == pingTimeoutTask)
                    {
                        Debug.WriteLine($"Ping timed-out({pingTimeoutMs / 1000}s). Closing connection.");
                        transport.Dispose(); //  kill connection so that Listening thread could finish and break connection

                        break;
                    }

                    await Task.Delay(pingDelayMs);
                }
                catch (Exception ex)
                {
                    if (!isClosed)
                    {
                        Debug.WriteLine($"Exception on Ping: {ex.Message}. Closing connection.");
                        transport.Dispose(); //  kill connection so that Listening thread could finish and break connection

                        break;
                    }
                }
            }
        }

        private async void StartListening()
        {
            Exception exception = null;
            finishedListening = new TaskCompletionSource<bool>();
            try
            {
                while (!isClosed)
                {
                    var message = await transport.Receieve().ConfigureAwait(false);
                    if (message == null)
                    {
                        Debug.WriteLine("Got termination package. Closing proto connection.");
                        break;
                    }

                    try
                    {
                        var decodedMessage = DecodeMessage(message.body);

                        using (var messageStream = new MemoryStream(decodedMessage.Item1, false))
                        using (var messageReader = new BinaryReader(messageStream))
                        {
                            ProcessMessage(decodedMessage.Item2, decodedMessage.Item3, messageReader);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Exception while handing message: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            var closedByClient = isClosed;

            CleanupConnection();
            finishedListening.SetResult(true);

            if (exception != null || !closedByClient)
            {
                OnBroken();
            }
        }

        private async Task SendPendingConfirmations()
        {
            if (needConfirmation.Count == 0)
                return;

            var requestsToConfirm = Interlocked.Exchange(ref needConfirmation, new List<long>());
            if (requestsToConfirm.Count > 0)
            {
                var ackRequest = new AckRequestLong(requestsToConfirm);
                using (var memory = new MemoryStream())
                using (var writer = new BinaryWriter(memory))
                {
                    ackRequest.MessageId = session.GetNewMessageId();

                    ackRequest.OnSend(writer);
                    await Send(memory.ToArray(), ackRequest);
                }
            }
        }

        private readonly object sendMessagesSyncRoot = new object();
        public async Task Send(MtProtoRequest request)
        {
            await SendPendingConfirmations();

            TaskCompletionSource<bool> responseSource;
            using (var memory = new MemoryStream())
            using (var writer = new BinaryWriter(memory))
            {
                request.MessageId = session.GetNewMessageId();
                Debug.WriteLine($"Send request - {request.GetType().Name} - {request.MessageId}");

                responseSource = new TaskCompletionSource<bool>();

                request.OnSend(writer);
                await Send(memory.ToArray(), request);

                lock (sendMessagesSyncRoot)
                {
                    if (isClosed)
                        throw exceptionForClosedConnection;

                    runningRequests.Add(request.MessageId, Tuple.Create(request, responseSource));
                }
            }

            await responseSource.Task;
            runningRequests.Remove(request.MessageId);
        }

        private async Task Send(byte[] packet, MtProtoRequest request)
        {
            byte[] msgKey;
            byte[] ciphertext;
            using (var plaintextPacket = Helpers.CreateMemoryStream(8 + 8 + 8 + 4 + 4 + packet.Length))
            {
                using (var plaintextWriter = new BinaryWriter(plaintextPacket))
                {
                    plaintextWriter.Write(session.salt);
                    plaintextWriter.Write(session.id);
                    plaintextWriter.Write(request.MessageId);
                    plaintextWriter.Write(session.GetNextSequenceNumber(request));
                    plaintextWriter.Write(packet.Length);
                    plaintextWriter.Write(packet);

                    msgKey = Helpers.CalcMsgKey(plaintextPacket.GetBuffer());
                    ciphertext = AES.EncryptAES(Helpers.CalcKey(session.authKey.Data, msgKey, true), plaintextPacket.GetBuffer());
                }
            }

            using (var ciphertextPacket = Helpers.CreateMemoryStream(8 + 16 + ciphertext.Length))
            {
                using (var writer = new BinaryWriter(ciphertextPacket))
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
                case 0x347773c5: // pong
                    Debug.WriteLine("Got Pong response from server.");
                    var pong = TLObject.Read<Pong>(messageReader, 0x347773c5);
                    if (runningRequests.ContainsKey(pong.messageId))
                    {
                        runningRequests[pong.messageId].Item2.TrySetResult(true);
                    }

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

        private void OnUpdateMessage(Updates update)
        {
            UpdateMessage?.Invoke(this, update);
        }
        private void OnBroken()
        {
            Broken?.Invoke(this, EventArgs.Empty);
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
            try
            {
                MtProtoRequest request = requestInfo.Item1;
                request.ConfirmReceived = true;

                uint innerCode = messageReader.ReadUInt32();
                if (innerCode == 0x2144ca19) // rpc_error
                {
                    int errorCode = messageReader.ReadInt32();
                    string errorMessage = Serializers.String.Read(messageReader);
                    request.OnError(errorCode, errorMessage);
                }
                else if (innerCode == 0x3072cfa1) // gzip_packed
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
                        }
                    }
                }
                else
                {
                    messageReader.BaseStream.Position -= 4;
                    request.OnResponse(messageReader);
                }

                requestInfo.Item2.SetResult(true);
            }
            catch (Exception ex)
            {
                requestInfo.Item2.SetException(ex);
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
            int badRequestSequence = messageReader.ReadInt32();
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

            Debug.WriteLine("New server session created");
            session.salt = serverSalt;
        }

        #endregion

        private void CleanupConnection()
        {
            lock (sendMessagesSyncRoot)
            {
                isClosed = true;
            }

            foreach (var request in runningRequests.ToList())
            {
                request.Value.Item2.TrySetException(exceptionForClosedConnection);
                runningRequests.Remove(request.Key);
            }

            if (runningRequests.Count > 0) // should never happen
            {
                Debug.WriteLine("Connection wasn't cleaned up properly!!");
            }
        }

        public void Dispose()
        {
            CleanupConnection();
            transport.Dispose();
        }
    }
}