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
using Telegram.Net.Core.Utils;
using MD5 = System.Security.Cryptography.MD5;

namespace Telegram.Net.Core
{
    public class TelegramClient
    {
        private static int apiLayer = 23;

        private static readonly string defaultServerAddress = "149.154.167.51";
        private static readonly int defaultServerPort = 443;

        private MtProtoSender protoSender;
        //private AuthKey key;
        private TcpTransport transport;
        private readonly string apiHash;
        private readonly int apiId;
        private readonly Session session;
        private List<DcOption> dcOptions;

        public int? authenticatedUserId => (session.user as UserSelfConstructor)?.id;

        public event EventHandler<Updates> UpdateMessage;

        public TelegramClient(ISessionStore store, int apiId, string apiHash, string serverAddress = null)
        {
            if (apiId == 0)
                throw new ArgumentException("API_ID is invalid", nameof(apiId));

            if (string.IsNullOrEmpty(apiHash))
                throw new ArgumentException("API_HASH is invalid", nameof(apiHash));

            this.apiHash = apiHash;
            this.apiId = apiId;

            serverAddress = serverAddress ?? defaultServerAddress;

            session = Session.TryLoadOrCreateNew(store, serverAddress, defaultServerPort);
            transport = new TcpTransport(session.serverAddress, session.port);
        }

        public async Task Connect(bool reconnect = false)
        {
            if (session.authKey == null || reconnect)
            {
                var result = await Authenticator.DoAuthentication(transport);
                session.authKey = result.AuthKey;
                session.timeOffset = result.TimeOffset;
            }

            protoSender = new MtProtoSender(transport, session);
            protoSender.UpdateMessage += OnUpdateMessage;

            var request = new SetLayerAndInitConnectionRequest(apiId, apiLayer);
            await SendRpcRequest(request);

            dcOptions = request.config.dcOptions;
        }

        private async Task ReconnectToDc(int dcId)
        {
            if (dcOptions == null || !dcOptions.Any())
                throw new InvalidOperationException("Can't reconnect. Establish initial connection first.");

            var dc = dcOptions.Cast<DcOptionConstructor>().First(d => d.id == dcId);

            await CloseCurrentTransport();

            transport = new TcpTransport(dc.ipAddress, dc.port);
            session.serverAddress = dc.ipAddress;
            session.port = dc.port;

            await Connect(true);
        }

        private async Task CloseCurrentTransport()
        {
            transport.Disconnect();

            await protoSender.finishedListeningTask;
            protoSender.UpdateMessage -= OnUpdateMessage;

            transport.Dispose();

            transport = null;
        }

        private void OnUserAuthenticated(User user, int sessionExpiration)
        {
            session.user = user;
            session.sessionExpires = sessionExpiration;

            session.Save();
        }

        protected virtual void OnUpdateMessage(object sender, Updates e)
        {
            UpdateMessage?.Invoke(this, e);
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

        #region Auth

        public bool IsUserAuthorized()
        {
            return session.user != null;
        }

        //auth.checkPhone#6fe51dfb phone_number:string = auth.CheckedPhone;
        public async Task<AuthCheckedPhoneConstructor> CheckPhone(string phoneNumber)
        {
            var authCheckPhoneRequest = new AuthCheckPhoneRequest(phoneNumber);
            await SendRpcRequest(authCheckPhoneRequest);

            // only single implementation available
            return (AuthCheckedPhoneConstructor)authCheckPhoneRequest.checkedPhone;
        }

        //auth.sendCode#768d5f4d phone_number:string sms_type:int api_id:int api_hash:string lang_code:string = auth.SentCode;
        public async Task<AuthSentCode> SendCode(string phoneNumber, VerificationCodeDeliveryType tokenDestination)
        {
            var request = new AuthSendCodeRequest(phoneNumber, (int)tokenDestination, apiId, apiHash, "en");
            await SendRpcRequest(request);

            return request.sentCode;
        }

        //auth.sendCall#3c51564 phone_number:string phone_code_hash:string = Bool;
        public async Task<bool> SendCall(string phoneNumber, string phoneCodeHash)
        {
            var request = new AuthSendCallRequest(phoneNumber, phoneCodeHash);
            await SendRpcRequest(request);

            return request.callSent;
        }

        //auth.signUp#1b067634 phone_number:string phone_code_hash:string phone_code:string first_name:string last_name:string = auth.Authorization;
        public async Task<AuthAuthorizationConstructor> SignUp(string phoneNumber, string phoneCodeHash, string code, string firstName, string lastName)
        {
            var request = new AuthSignUpRequest(phoneNumber, phoneCodeHash, code, firstName, lastName);
            await SendRpcRequest(request);

            // only single implementation available
            var authorization = (AuthAuthorizationConstructor)request.authorization;
            OnUserAuthenticated(authorization.user, authorization.expires);

            return authorization;
        }

        //auth.signIn#bcd51581 phone_number:string phone_code_hash:string phone_code:string = auth.Authorization;
        public async Task<AuthAuthorizationConstructor> SignIn(string phoneNumber, string phoneCodeHash, string code)
        {
            var request = new AuthSignInRequest(phoneNumber, phoneCodeHash, code);
            await SendRpcRequest(request);

            // only single implementation available
            var authorization = (AuthAuthorizationConstructor)request.authorization;
            OnUserAuthenticated(authorization.user, authorization.expires);

            return authorization;
        }

        // TODO
        // auth.logOut#5717da40 = Bool;
        // auth.resetAuthorizations#9fab0d1a = Bool;
        // auth.sendInvites#771c1d97 phone_numbers:Vector<string> message:string = Bool;
        // auth.exportAuthorization#e5bfffcd dc_id:int = auth.ExportedAuthorization;
        // auth.importAuthorization#e3ef9613 id:int bytes:bytes = auth.Authorization;
        // auth.bindTempAuthKey#cdd42a05 perm_auth_key_id:long nonce:long expires_at:int encrypted_message:bytes = Bool;

        //auth.sendSms#da9f3e8 phone_number:string phone_code_hash:string = Bool;
        public async Task<bool> SendSms(string phoneNumber, string phoneCodeHash)
        {
            var request = new AuthSendSmsRequest(phoneNumber, phoneCodeHash);
            await SendRpcRequest(request);

            return request.smsSent;
        }

        #endregion

        #region Account

        // TODO
        //account.registerDevice#637ea878 token_type:int token:string = Bool;
        //account.unregisterDevice#65c55b40 token_type:int token:string = Bool;
        //account.updateNotifySettings#84be5b93 peer:InputNotifyPeer settings:InputPeerNotifySettings = Bool;
        //account.getNotifySettings#12b3ad31 peer:InputNotifyPeer = PeerNotifySettings;
        //account.resetNotifySettings#db7e1747 = Bool;

        // account.updateProfile#78515775 flags:# first_name:flags.0?string last_name:flags.1?string about:flags.2?string = User;
        public async Task<User> UpdateProfile(string first_name, string last_name)
        {
            var request = new UpdateProfileRequest(first_name, last_name);
            await SendRpcRequest(request);

            return request.UserResponse;
        }

        //account.updateStatus#6628562c offline:Bool = Bool;
        //account.getWallPapers#c04cfac2 = Vector<WallPaper>;
        //account.reportPeer#ae189d5f peer:InputPeer reason:ReportReason = Bool;
        //account.checkUsername#2714d86c username:string = Bool;
        //account.updateUsername#3e0bdd7c username:string = User;
        //account.getPrivacy#dadbc950 key:InputPrivacyKey = account.PrivacyRules;
        //account.setPrivacy#c9f81ce8 key:InputPrivacyKey rules:Vector<InputPrivacyRule> = account.PrivacyRules;
        //account.deleteAccount#418d4e0b reason:string = Bool;
        //account.getAccountTTL#8fc711d = AccountDaysTTL;
        //account.setAccountTTL#2442485e ttl:AccountDaysTTL = Bool;
        //account.sendChangePhoneCode#8e57deb flags:# allow_flashcall:flags.0?true phone_number:string current_number:flags.0?Bool = auth.SentCode;
        //account.changePhone#70c32edb phone_number:string phone_code_hash:string phone_code:string = User;
        //account.updateDeviceLocked#38df3532 period:int = Bool;
        //account.getAuthorizations#e320c158 = account.Authorizations;
        //account.resetAuthorization#df77f3bc hash:long = Bool;
        //account.getPassword#548a30f5 = account.Password;
        //account.getPasswordSettings#bc8d11bb current_password_hash:bytes = account.PasswordSettings;
        //account.updatePasswordSettings#fa7c4b86 current_password_hash:bytes new_settings:account.PasswordInputSettings = Bool;
        //account.sendConfirmPhoneCode#1516d7bd flags:# allow_flashcall:flags.0?true hash:string current_number:flags.0?Bool = auth.SentCode;
        //account.confirmPhone#5f2178c3 phone_code_hash:string phone_code:string = Bool;

        #endregion

        #region Users

        // users.getUsers#d91a548 id:Vector<InputUser> = Vector<User>;
        public async Task<List<User>> GetUsers(List<InputUser> ids)
        {
            var request = new GetUsersRequest(ids);
            await SendRpcRequest(request);

            return request.users;
        }

        // users.getFullUser#ca30a5b1 id:InputUser = UserFull;
        public async Task<UserFullConstructor> GetFullUser(InputUser user)
        {
            var request = new GetFullUserRequest(user);
            await SendRpcRequest(request);

            // only single implementation available
            return (UserFullConstructor)request.userFull;
        }

        #endregion

        #region Contacts

        // TODO
        // contacts.getStatuses#c4a353ee = Vector<ContactStatus>;

        // contacts.getContacts#22c6aa08 hash:string = contacts.Contacts;
        public async Task<ContactsContacts> GetContacts(IEnumerable<int> alreadyLoadedContactsIds = null)
        {
            var request = new GetContactsRequest(alreadyLoadedContactsIds);
            await SendRpcRequest(request);

            return request.contacts;
        }

        // contacts.importContacts#da30b32d contacts:Vector<InputContact> replace:Bool = contacts.ImportedContacts;
        public async Task<ContactsImportedContactsConstructor> ImportContactByPhoneNumber(string phoneNumber, string firstName, string lastName, bool replace = true)
        {
            var inputContact = new InputPhoneContactConstructor(0, phoneNumber, firstName, lastName);
            var request = new ImportContactsRequest(new List<InputContact> { inputContact }, replace);
            await SendRpcRequest(request);

            // only single implementation available
            return (ContactsImportedContactsConstructor)request.importedContacts;
        }

        // contacts.deleteContact#8e953744 id:InputUser = contacts.Link;
        // contacts.deleteContacts#59ab389e id:Vector<InputUser> = Bool;
        // contacts.block#332b49fc id:InputUser = Bool;
        // contacts.unblock#e54100bd id:InputUser = Bool;
        // contacts.getBlocked#f57c350f offset:int limit:int = contacts.Blocked;
        // contacts.exportCard#84e53737 = Vector<int>;
        // contacts.importCard#4fe196fe export_card:Vector<int> = User;
        // contacts.search#11f812d8 q:string limit:int = contacts.Found;

        public async Task<User> ResolveUsername(string username)
        {
            var request = new ResolveUsernameRequest(username);
            await SendRpcRequest(request);

            return request.user;
        }

        #endregion

        #region Messages

        // messages.getMessages#4222fa74 id:Vector<int> = messages.Messages;

        // messages.getDialogs#eccf1df6 offset:int max_id:int limit:int = messages.Dialogs;
        public async Task<MessagesDialogs> GetDialogs(int offset, int limit, int maxId = 0)
        {
            var request = new GetDialogsRequest(offset, maxId, limit);
            await SendRpcRequest(request);

            return request.messagesDialogs;
        }

        // messages.getHistory#92a1df2f peer:InputPeer offset:int max_id:int limit:int = messages.Messages;
        public async Task<MessagesMessages> GetHistoryForContact(int userId, int offset, int limit, int maxId = -1)
        {
            var request = new GetHistoryRequest(new InputPeerContactConstructor(userId), offset, maxId, limit);
            await SendRpcRequest(request);

            return request.messages;
        }
        public async Task<MessagesMessages> GetHistoryForForeignContact(int userId, long accessHash, int offset, int limit, int maxId = -1)
        {
            var request = new GetHistoryRequest(new InputPeerForeignConstructor(userId, accessHash), offset, maxId, limit);
            await SendRpcRequest(request);

            return request.messages;
        }

        // messages.search#7e9f2ab peer:InputPeer q:string filter:MessagesFilter min_date:int max_date:int offset:int max_id:int limit:int = messages.Messages;
        // messages.readHistory#eed884c6 peer:InputPeer max_id:int offset:int read_contents:Bool = messages.AffectedHistory;
        // messages.deleteHistory#f4f8fb61 peer:InputPeer offset:int = messages.AffectedHistory;

        // messages.deleteMessages#14f2dd0a id:Vector<int> = Vector<int>;
        public async Task<List<int>> DeleteMessages(List<int> messageIdsToDelete)
        {
            var request = new DeleteMessagesRequest(messageIdsToDelete);
            await SendRpcRequest(request);

            return request.deletedMessageIds;
        }

        // messages.receivedMessages#28abcb68 max_id:int = Vector<int>;
        // messages.setTyping#a3825e50 peer:InputPeer action:SendMessageAction = Bool;
        public async Task<bool> SetTyping(InputPeer inputPeer, SendMessageAction action)
        {
            var request = new SetTypingRequest(inputPeer, action);
            await SendRpcRequest(request);

            return request.state;
        }

        // messages.sendMessage#4cde0aab peer:InputPeer message:string random_id:long = messages.SentMessage;
        public async Task<SentMessage> SendMessage(InputPeer inputPeer, string message)
        {
            var request = new SendMessageRequest(inputPeer, message);
            await SendRpcRequest(request);

            return request.sentMessage;
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

        // messages.sendMedia#a3c85d76 peer:InputPeer media:InputMedia random_id:long = messages.StatedMessage;
        public async Task<MessagesStatedMessage> SendMediaMessage(InputPeer inputPeer, InputMedia media)
        {
            var request = new SendMediaRequest(inputPeer, media);
            await SendRpcRequest(request);

            return request.statedMessage;
        }

        // messages.forwardMessages#514cd10f peer:InputPeer id:Vector<int> = messages.StatedMessages;
        // messages.reportSpam#cf1592db peer:InputPeer = Bool;
        // messages.hideReportSpam#a8f1709b peer:InputPeer = Bool;
        // messages.getPeerSettings#3672e09c peer:InputPeer = PeerSettings;
        // messages.getChats#3c6aa187 id:Vector<int> = messages.Chats;
        // messages.getFullChat#3b831c66 chat_id:int = messages.ChatFull;
        // messages.editChatTitle#b4bc68b5 chat_id:int title:string = messages.StatedMessage;
        // messages.editChatPhoto#d881821d chat_id:int photo:InputChatPhoto = messages.StatedMessage;

        // messages.addChatUser#2ee9ee9e chat_id:int user_id:InputUser fwd_limit:int = messages.StatedMessage;
        public async Task<MessagesStatedMessage> AddChatUser(int chatId, InputUser user, int messagesToForward)
        {
            var request = new AddChatUserRequest(chatId, user, messagesToForward);
            await SendRpcRequest(request);

            return request.statedMessage;
        }

        // messages.deleteChatUser#c3c5cd23 chat_id:int user_id:InputUser = messages.StatedMessage;
        public async Task<MessagesStatedMessage> DeleteChatUser(int chatId, InputUser userToDelete)
        {
            var request = new DeleteChatUserRequest(chatId, userToDelete);
            await SendRpcRequest(request);

            return request.statedMessage;
        }
        public async Task<MessagesStatedMessage> LeaveChat(int chatId)
        {
            if (!authenticatedUserId.HasValue)
                throw new Exception("Not authorized");

            var request = new DeleteChatUserRequest(chatId, new InputUserContactConstructor(authenticatedUserId.Value));
            await SendRpcRequest(request);

            return request.statedMessage;
        }

        // messages.createChat#419d9aee users:Vector<InputUser> title:string = messages.StatedMessage;
        public async Task<MessagesStatedMessage> CreateChat(string title, List<InputUser> usersToInvite)
        {
            var request = new CreateChatRequest(usersToInvite, title);
            await SendRpcRequest(request);

            return request.statedMessage;
        }

        // messages.forwardMessage#3f3f4f2 peer:InputPeer id:int random_id:long = messages.StatedMessage;
        // messages.getDhConfig#26cf8950 version:int random_length:int = messages.DhConfig;
        // messages.requestEncryption#f64daf43 user_id:InputUser random_id:int g_a:bytes = EncryptedChat;
        // messages.acceptEncryption#3dbc0415 peer:InputEncryptedChat g_b:bytes key_fingerprint:long = EncryptedChat;
        // messages.discardEncryption#edd923c5 chat_id:int = Bool;
        // messages.setEncryptedTyping#791451ed peer:InputEncryptedChat typing:Bool = Bool;
        // messages.readEncryptedHistory#7f4b690a peer:InputEncryptedChat max_date:int = Bool;
        // messages.sendEncrypted#a9776773 peer:InputEncryptedChat random_id:long data:bytes = messages.SentEncryptedMessage;
        // messages.sendEncryptedFile#9a901b66 peer:InputEncryptedChat random_id:long data:bytes file:InputEncryptedFile = messages.SentEncryptedMessage;
        // messages.sendEncryptedService#32d439a4 peer:InputEncryptedChat random_id:long data:bytes = messages.SentEncryptedMessage;
        // messages.receivedQueue#55a5bb66 max_qts:int = Vector<long>;
        // messages.readMessageContents#354b5bc2 id:Vector<int> = Vector<int>;
        // messages.getStickers#ae22e045 emoticon:string hash:string = messages.Stickers;
        // messages.getAllStickers#aa3bc868 hash:string = messages.AllStickers;

        #endregion

        #region Updates

        // updates.getState#edd4882a = updates.State;
        public async Task<UpdatesStateConstructor> GetUpdatesState()
        {
            var request = new GetUpdatesStateRequest();
            await SendRpcRequest(request);

            // only single implementation available
            return (UpdatesStateConstructor)request.updatesState;
        }

        // updates.getDifference#a041495 pts:int date:int qts:int = updates.Difference;
        public async Task<UpdatesDifference> GetUpdatesDifference(int pts, int date, int qts)
        {
            var request = new GetUpdatesDifferenceRequest(pts, date, qts);
            await SendRpcRequest(request);

            // only single implementation available
            return request.updatesDifference;
        }

        #endregion

        #region Photos

        // photos.updateProfilePhoto#eef579a0 id:InputPhoto crop:InputPhotoCrop = UserProfilePhoto;
        // photos.uploadProfilePhoto#d50f9c88 file:InputFile caption:string geo_point:InputGeoPoint crop:InputPhotoCrop = photos.Photo;
        // photos.deletePhotos#87cf7f2f id:Vector<InputPhoto> = Vector<long>;
        // photos.getUserPhotos#b7ee553c user_id:InputUser offset:int max_id:int limit:int = photos.Photos;

        #endregion

        #region Upload

        // upload.saveFilePart#b304a621 file_id:long file_part:int bytes:bytes = Bool;
        public async Task<bool> SaveFilePart(long fileId, int filePart, byte[] bytes)
        {
            var request = new SaveFilePartRequest(fileId, filePart, bytes, 0, bytes.Length);
            await SendRpcRequest(request);

            return request.done;
        }
        public async Task<InputFileConstructor> UploadFile(string name, Stream content)
        {
            var buffer = new byte[65536];
            var fileId = BitConverter.ToInt64(Helpers.GenerateRandomBytes(8), 0);

            int partsCount = 0;
            int bytesRead;
            while ((bytesRead = content.Read(buffer, 0, buffer.Length)) > 0)
            {
                var request = new SaveFilePartRequest(fileId, partsCount, buffer, 0, bytesRead);
                await SendRpcRequest(request);

                partsCount++;

                if (request.done == false)
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

        // upload.getFile#e3a6cfb5 location:InputFileLocation offset:int limit:int = upload.File;
        public async Task<UploadFileConstructor> GetFile(long volumeId, int localId, long secret, int offset, int limit)
        {
            var request = new GetFileRequest(new InputFileLocationConstructor(volumeId, localId, secret), offset, limit);
            await SendRpcRequest(request);

            // only single implementation available
            return (UploadFileConstructor)request.file;
        }
        public async Task<UploadFileConstructor> GetFile(InputFileLocation fileLocation, int offset, int limit)
        {
            var request = new GetFileRequest(fileLocation, offset, limit);
            await SendRpcRequest(request);

            // only single implementation available
            return (UploadFileConstructor)request.file;
        }

        // upload.saveBigFilePart#de7b673d file_id:long file_part:int file_total_parts:int bytes:bytes = Bool;

        #endregion

        #region Help

        // help.getConfig#c4f9186b = Config;
        // help.getNearestDc#1fb33026 = NearestDc;
        // help.getAppUpdate#c812ac7e device_model:string system_version:string app_version:string lang_code:string = help.AppUpdate;
        // help.saveAppLog#6f02f748 events:Vector<InputAppEvent> = Bool;
        // help.getInviteText#a4a95186 lang_code:string = help.InviteText;
        // help.getSupport#9cdf08cd = help.Support;

        #endregion

        public Task Close()
        {
            return CloseCurrentTransport();
        }
    }
}