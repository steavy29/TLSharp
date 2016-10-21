using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Net.Core.Auth;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.Network;
using Telegram.Net.Core.Requests;
using MD5 = System.Security.Cryptography.MD5;

namespace Telegram.Net.Core
{
    public class TelegramClient
    {
        private static readonly string defaultServerAddress = "149.154.167.50";
        private static readonly int defaultServerPort = 443;

        private MtProtoSender protoSender;
        //private AuthKey key;
        private TcpTransport transport;
        private readonly string apiHash;
        private readonly int apiId;
        private readonly Session session;
        private List<DcOption> dcOptions;

        public int? authenticatedUserId => (session.User as UserSelfConstructor)?.id;

        public event EventHandler<Updates> UpdateMessage;

        public TelegramClient(ISessionStore store, int apiId, string apiHash, string serverAddress = null)
        {
            if (apiId == 0)
                throw new InvalidOperationException("Your API_ID is invalid. Do a configuration first https://github.com/sochix/TLSharp#quick-configuration");

            if (string.IsNullOrEmpty(apiHash))
                throw new InvalidOperationException("Your API_ID is invalid. Do a configuration first https://github.com/sochix/TLSharp#quick-configuration");

            this.apiHash = apiHash;
            this.apiId = apiId;

            serverAddress = serverAddress ?? defaultServerAddress;

            session = Session.TryLoadOrCreateNew(store, serverAddress, defaultServerPort);
            transport = new TcpTransport(session.ServerAddress, session.Port);
        }

        public async Task Connect(bool reconnect = false)
        {
            if (session.AuthKey == null || reconnect)
            {
                var result = await Authenticator.DoAuthentication(transport);
                session.AuthKey = result.AuthKey;
                session.TimeOffset = result.TimeOffset;
            }

            protoSender = new MtProtoSender(transport, session);
            protoSender.UpdateMessage += OnUpdateMessage;

            var request = new InitConnectionRequest(apiId);
            await SendRpcRequest(request);

            dcOptions = request.ConfigConstructor.dc_options;
        }

        private async Task ReconnectToDc(int dcId)
        {
            if (dcOptions == null || !dcOptions.Any())
                throw new InvalidOperationException("Can't reconnect. Establish initial connection first.");

            var dc = dcOptions.Cast<DcOptionConstructor>().First(d => d.id == dcId);

            await CloseCurrentTransport();

            transport = new TcpTransport(dc.ip_address, dc.port);
            session.ServerAddress = dc.ip_address;
            session.Port = dc.port;

            await Connect(true);
        }

        private async Task CloseCurrentTransport()
        {
            transport.Disconnect();

            await protoSender.FinishedListeningTask;
            protoSender.UpdateMessage -= OnUpdateMessage;

            transport.Dispose();

            transport = null;
        }

        public bool IsUserAuthorized()
        {
            return session.User != null;
        }

        public async Task<auth_CheckedPhone> CheckPhone(string phoneNumber)
        {
            var authCheckPhoneRequest = new AuthCheckPhoneRequest(phoneNumber);
            await SendRpcRequest(authCheckPhoneRequest);

            return authCheckPhoneRequest.checkedPhone;
        }

        public async Task<AuthSentCode> SendCodeRequest(string phoneNumber, VerificationCodeDeliveryType tokenDestination = VerificationCodeDeliveryType.NumericCodeViaTelegram)
        {
            var request = new AuthSendCodeRequest(phoneNumber, (int)tokenDestination, apiId, apiHash, "en");
            await SendRpcRequest(request);

            return request.sentCode;
        }

        public async Task<User> MakeAuth(string phoneNumber, string phoneCodeHash, string code)
        {
            var request = new AuthSignInRequest(phoneNumber, phoneCodeHash, code);
            await SendRpcRequest(request);

            OnUserAuthenticated(request.user, request.SessionExpires);

            return request.user;
        }

        public async Task<User> SignUp(string phoneNumber, string phoneCodeHash, string code, string firstName, string lastName)
        {
            var request = new AuthSignUpRequest(phoneNumber, phoneCodeHash, code, firstName, lastName);
            await SendRpcRequest(request);

            OnUserAuthenticated(request.user, request.SessionExpires);

            return request.user;
        }

        private void OnUserAuthenticated(User user, int sessionExpiration)
        {
            session.User = user;
            session.SessionExpires = sessionExpiration;

            session.Save();
        }

        public async Task<InputFileConstructor> UploadFile(string name, Stream content)
        {
            var buffer = new byte[65536];
            var fileId = DateTime.Now.Ticks;

            int partsCount = 0;
            int bytesRead;
            while((bytesRead = content.Read(buffer, 0, buffer.Length)) > 0)
            {
                var request = new Upload_SaveFilePartRequest(fileId, partsCount, buffer, bytesRead);
                await SendRpcRequest(request);

                partsCount++;

                if (request.Done == false)
                    throw new InvalidOperationException($"Failed to upload file({name}) part: {partsCount})");
            }

            var md5Checksum = string.Empty;
            if (content.CanSeek)
            {
                content.Position = 0;
                
                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(content);
                    var hashResult = new StringBuilder(hash.Length * 2);

                    foreach (byte b in hash)
                        hashResult.Append(b.ToString("x2"));

                    md5Checksum = hashResult.ToString();
                }
            }

            return new InputFileConstructor(fileId, partsCount, name, md5Checksum);
        }

        public async Task<messages_StatedMessage> SendMediaMessage(InputPeer inputPeer, InputMedia media)
        {
            var request = new Message_SendMediaRequest(inputPeer, media);
            await SendRpcRequest(request);

            return request.StatedMessage;
        }

        public async Task<ContactsImportedContactsConstructor> ImportContactByPhoneNumber(string phoneNumber, string firstName, string lastName, bool replace = true)
        {
            var request = new ImportContactRequest(new InputPhoneContactConstructor(0, phoneNumber, firstName, lastName), replace);
            await SendRpcRequest(request);

            return (ContactsImportedContactsConstructor)request.importedContacts;
        }

        public async Task<SentMessage> SendDirectMessage(int userId, string message)
        {
            var request = new SendMessageRequest(new InputPeerContactConstructor(userId), message);
            await SendRpcRequest(request);

            return request.sentMessage;
        }

        public async Task<SentMessage> SendChatMessage(int chatId, string message)
        {
            var request = new SendMessageRequest(new InputPeerChatConstructor(chatId), message);
            await SendRpcRequest(request);

            return request.sentMessage;
        }

        public async Task<SentMessage> SendMessageToForeignContact(int id, long accessHash, string message)
        {
            var request = new SendMessageRequest(new InputPeerForeignConstructor(id, accessHash), message);
            await SendRpcRequest(request);

            return request.sentMessage;
        }

        public async Task<List<int>> DeleteMessages(List<int> messageIdsToDelete)
        {
            var request = new DeleteMessagesRequest(messageIdsToDelete);
            await SendRpcRequest(request);

            return request.deletedMessageIds;
        }

        public async Task<List<Message>> GetMessagesHistoryForContact(int userId, int offset, int limit, int maxId = -1)
        {
            var request = new GetHistoryRequest(new InputPeerContactConstructor(userId), offset, maxId, limit);
            await SendRpcRequest(request);

            return request.messages;
        }

        public async Task<List<Message>> GetMessagesHistoryForForeignContact(int userId, long accessHash, int offset, int limit, int maxId = -1)
        {
            var request = new GetHistoryRequest(new InputPeerForeignConstructor(userId, accessHash), offset, maxId, limit);
            await SendRpcRequest(request);

            return request.messages;
        }

        public async Task<Tuple<storage_FileType, byte[]>> GetFile(long volumeId, int localId, long secret, int offset, int limit)
        {
            var request = new GetFileRequest(new InputFileLocationConstructor(volumeId, localId, secret), offset, limit);
            await SendRpcRequest(request);

            return Tuple.Create(request.type, request.bytes);
        }

        public async Task<MessageDialogs> GetDialogs(int offset, int limit, int maxId = 0)
        {
            var request = new GetDialogsRequest(offset, maxId, limit);
            await SendRpcRequest(request);

            return new MessageDialogs
            {
                Dialogs = request.dialogs,
                Messages = request.messages,
                Chats = request.chats,
                Users = request.users
            };
        }

        public async Task<Messages_statedMessageConstructor> CreateChat(string title, List<InputUser> usersToInvite)
        {
            var request = new CreateChatRequest(usersToInvite, title);
            await SendRpcRequest(request);

            return request.message;
        }

        public async Task<Messages_statedMessageConstructor> LeaveChat(int chatId)
        {
            var request = new DeleteChatUserRequest(chatId, new InputUserContactConstructor(authenticatedUserId.Value));
            await SendRpcRequest(request);

            return request.message;
        }

        protected virtual void OnUpdateMessage(object sender, Updates e)
        {
            UpdateMessage?.Invoke(this, e);
        }

        public Task Close()
        {
            return CloseCurrentTransport();
        }

        public async Task SendRpcRequest(MTProtoRequest request)
        {
            await protoSender.Send(request);

            // error handling order is important

            if (request.Error == RpcRequestError.MigrateDataCenter)
            {
                if (request.ErrorMessage.StartsWith("PHONE_MIGRATE_") ||
                    request.ErrorMessage.StartsWith("NETWORK_MIGRATE_") ||
                    request.ErrorMessage.StartsWith("USER_MIGRATE_"))
                {
                    var dcIdStr = Regex.Match(request.ErrorMessage, @"\d+").Value;
                    var dcId = int.Parse(dcIdStr);

                    await ReconnectToDc(dcId);

                    // try one more time
                    request.ResetError();
                    await protoSender.Send(request);
                }
            }

            if (request.Error == RpcRequestError.Flood)
            {
                if (request.ErrorMessage.StartsWith("FLOOD_WAIT_"))
                {
                    var secondsToWaitStr = Regex.Match(request.ErrorMessage, @"\d+").Value;
                    var secondsToWait = int.Parse(secondsToWaitStr);

                    if (secondsToWait <= 2)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(secondsToWait));

                        // try one more time
                        request.ResetError();
                        await protoSender.Send(request);
                    }
                    
                    // otherwise error and exception
                }
            }

            // handle errors that can be fixed without user interaction
            if (request.Error == RpcRequestError.IncorrectServerSalt)
            {
                // assuming that salt was already updated by underlying layer
                request.ResetError();
                await protoSender.Send(request);
            }

            if (request.Error == RpcRequestError.MessageSeqNoTooLow)
            {
                // resync updates state
            }

            session.Save();

            // escalate to user
            request.ThrowIfHasError();
        }
    }
}