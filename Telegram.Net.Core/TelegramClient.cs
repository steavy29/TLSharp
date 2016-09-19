using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Net.Core.Auth;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.MTProto.Crypto;
using Telegram.Net.Core.Network;
using Telegram.Net.Core.Requests;
using MD5 = System.Security.Cryptography.MD5;

namespace Telegram.Net.Core
{
    public class TelegramClient
    {
        private MtProtoSender protoSender;
        private AuthKey key;
        private TcpTransport transport;
        private readonly string apiHash;
        private readonly int apiId;
        private readonly Session session;
        private List<DcOption> dcOptions;

        public event EventHandler<Updates> UpdateMessage;

        public enum VerificationCodeDeliveryType
        {
            NumericCodeViaSms = 0,
            NumericCodeViaTelegram = 5
        }

        public TelegramClient(ISessionStore store, string sessionUserId, int apiId, string apiHash)
        {
            if (apiId == 0)
                throw new InvalidOperationException("Your API_ID is invalid. Do a configuration first https://github.com/sochix/TLSharp#quick-configuration");

            if (string.IsNullOrEmpty(apiHash))
                throw new InvalidOperationException("Your API_ID is invalid. Do a configuration first https://github.com/sochix/TLSharp#quick-configuration");

            this.apiHash = apiHash;
            this.apiId = apiId;
            session = Session.TryLoadOrCreateNew(store, sessionUserId);
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

            if (!reconnect)
            {
                var request = new InitConnectionRequest(apiId);
                await SendRpcRequest(request);

                dcOptions = request.ConfigConstructor.dc_options;
            }
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

        public async Task<bool> IsPhoneRegistered(string phoneNumber)
        {
            var authCheckPhoneRequest = new AuthCheckPhoneRequest(phoneNumber);
            await SendRpcRequest(authCheckPhoneRequest);

            return authCheckPhoneRequest._phoneRegistered;
        }

        public async Task<string> SendCodeRequest(string phoneNumber, VerificationCodeDeliveryType tokenDestination = VerificationCodeDeliveryType.NumericCodeViaTelegram)
        {
            var request = new AuthSendCodeRequest(phoneNumber, (int)tokenDestination, apiId, apiHash, "en");
            await SendRpcRequest(request);

            return request._phoneCodeHash;
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

        public async Task<InputFile> UploadFile(string name, byte[] data)
        {
            var partSize = 65536;

            var fileId = DateTime.Now.Ticks;

            var partedData = new Dictionary<int, byte[]>();
            var partsCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(data.Length) / Convert.ToDouble(partSize)));
            var remainBytes = data.Length;
            for (int i = 0; i < partsCount; i++)
            {
                partedData.Add(i, data
                    .Skip(i * partSize)
                    .Take(remainBytes < partSize ? remainBytes : partSize)
                    .ToArray());

                remainBytes -= partSize;
            }

            for (int i = 0; i < partsCount; i++)
            {
                var saveFilePartRequest = new Upload_SaveFilePartRequest(fileId, i, partedData[i]);
                await SendRpcRequest(saveFilePartRequest);

                if (saveFilePartRequest.Done == false)
                    throw new InvalidOperationException($"Failed to upload fine. (failed part: {i}/{partsCount})");
            }

            string md5Checksum;
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(data);
                var hashResult = new StringBuilder(hash.Length * 2);

                for (int i = 0; i < hash.Length; i++)
                    hashResult.Append(i.ToString("x2"));

                md5Checksum = hashResult.ToString();
            }

            return new InputFileConstructor(fileId, partsCount, name, md5Checksum);
        }

        public async Task<bool> SendMediaMessage(int contactId, InputFile file)
        {
            var request = new Message_SendMediaRequest(
                new InputPeerContactConstructor(contactId),
                new InputMediaUploadedPhotoConstructor(file));

            await SendRpcRequest(request);

            return true;
        }

        public async Task<int?> ImportContactByPhoneNumber(string phoneNumber)
        {
            if (!validateNumber(phoneNumber))
                throw new InvalidOperationException("Invalid phone number. It should be only digit string, from 5 to 20 digits.");

            var request = new ImportContactRequest(new InputPhoneContactConstructor(0, phoneNumber, "My Test Name", String.Empty));
            await SendRpcRequest(request);

            var importedUser = (ImportedContactConstructor)request.imported.FirstOrDefault();

            return importedUser?.user_id;
        }

        public async Task<int?> ImportByUserName(string username)
        {
            var request = new ImportByUserName(username);
            await SendRpcRequest(request);

            return request.id;
        }

        public async Task SendMessage(int id, string message)
        {
            var request = new SendMessageRequest(new InputPeerContactConstructor(id), message);
            await SendRpcRequest(request);
        }

        public async Task<List<Message>> GetMessagesHistoryForContact(int userId, int offset, int limit, int maxId = -1)
        {
            var request = new GetHistoryRequest(new InputPeerContactConstructor(userId), offset, maxId, limit);
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

        public async Task<UserFull> GetUserFull(int userId)
        {
            var request = new GetUserFullRequest(userId);
            await SendRpcRequest(request);

            return request._userFull;
        }

        private bool validateNumber(string number)
        {
            var regex = new Regex("^\\d{7,20}$");
            return regex.IsMatch(number);
        }

        public async Task<ContactsContacts> GetContacts(IList<int> contactIds = null)
        {
            var request = new GetContactsRequest(contactIds);
            await SendRpcRequest(request);

            return new ContactsContacts
            {
                Contacts = request.Contacts,
                Users = request.Users
            };
        }

        public async Task<Messages_statedMessageConstructor> CreateChat(string title, List<string> userPhonesToInvite)
        {
            var userIdsToInvite = new List<int>();
            foreach (var userPhone in userPhonesToInvite)
            {
                var uid = await ImportContactByPhoneNumber(userPhone);
                if (!uid.HasValue)
                    throw new InvalidOperationException($"Failed to retrieve contact {userPhone}");

                userIdsToInvite.Add(uid.Value);
            }

            return await CreateChat(title, userIdsToInvite);
        }

        public async Task<Messages_statedMessageConstructor> CreateChat(string title, List<int> userIdsToInvite)
        {
            var request = new CreateChatRequest(userIdsToInvite.Select(uid => new InputUserContactConstructor(uid)).ToList(), title);
            await protoSender.Send(request);

            return request.message;
        }

        public async Task<Messages_statedMessageConstructor> AddChatUser(int chatId, int userId)
        {
            var request = new AddChatUserRequest(chatId, new InputUserContactConstructor(userId));
            await protoSender.Send(request);

            return request.message;
        }

        public async Task<Messages_statedMessageConstructor> DeleteChatUser(int chatId, int userId)
        {
            var request = new DeleteChatUserRequest(chatId, new InputUserContactConstructor(userId));
            await protoSender.Send(request);

            return request.message;
        }

        public async Task<Messages_statedMessageConstructor> LeaveChat(int chatId)
        {
            return await DeleteChatUser(chatId, ((UserSelfConstructor)session.User).id);
        }

        public async Task<updates_State> GetUpdatesState()
        {
            var request = new GetUpdatesStateRequest();
            await protoSender.Send(request);

            return request.updates;
        }

        public async Task<updates_Difference> GetUpdatesDifference(int lastPts, int lastDate, int lastQts)
        {
            var request = new GetUpdatesDifferenceRequest(lastPts, lastDate, lastQts);
            await protoSender.Send(request);

            return request.updatesDifference;
        }
        protected virtual void OnUpdateMessage(object sender, Updates e)
        {
            UpdateMessage?.Invoke(this, e);
        }

        public Task Close()
        {
            return CloseCurrentTransport();
        }

        private async Task SendRpcRequest(MTProtoRequest request)
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

                    await Task.Delay(TimeSpan.FromSeconds(secondsToWait));

                    // try one more time
                    request.ResetError();
                    await protoSender.Send(request);
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

            // escalate to user
            if (request.Error != RpcRequestError.None)
            {
                throw new Exception($"{request.Error} - {request.ErrorMessage}");
            }

            session.Save();
        }
    }
}