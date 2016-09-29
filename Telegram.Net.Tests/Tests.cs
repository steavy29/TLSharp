using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telegram.Net.Core;
using Telegram.Net.Core.Auth;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.Network;

namespace Telegram.Net.Tests
{
    [TestClass]
    public class Tests
    {
        private int ApiId { get; set; }

        private string ApiHash { get; set; }

        private string NumberToSendMessage { get; set; }

        private string NumberToAuthenticate { get; set; }

        private string NotRegisteredNumberToSignUp { get; set; }

        private string UserNameToSendMessage { get; set; }

        private string NumberToGetUserFull { get; set; }

        private string NumberToAddToChat { get; set; }
        
        [TestInitialize]
        public void Init()
        {
            // Setup your api info and phone numbers in app.config

            ApiId = int.Parse(ConfigurationManager.AppSettings[nameof(ApiId)]);
            ApiHash = ConfigurationManager.AppSettings[nameof(ApiHash)];
            if (string.IsNullOrEmpty(ApiHash))
                Debug.WriteLine("ApiHash not configured in app.config! Some tests may fail.");
            
            NumberToAuthenticate = ConfigurationManager.AppSettings[nameof(NumberToAuthenticate)];
            if (string.IsNullOrEmpty(NumberToAuthenticate))
                Debug.WriteLine("NumberToAuthenticate not configured in app.config! Some tests may fail.");

            NotRegisteredNumberToSignUp = ConfigurationManager.AppSettings[nameof(NotRegisteredNumberToSignUp)];
            if (string.IsNullOrEmpty(NotRegisteredNumberToSignUp))
                Debug.WriteLine("NotRegisteredNumberToSignUp not configured in app.config! Some tests may fail.");

            NumberToSendMessage = ConfigurationManager.AppSettings[nameof(NumberToSendMessage)];
            if (string.IsNullOrEmpty(NumberToSendMessage))
                Debug.WriteLine("NumberToSendMessage not configured in app.config! Some tests may fail.");

            UserNameToSendMessage = ConfigurationManager.AppSettings[nameof(UserNameToSendMessage)];
            if (string.IsNullOrEmpty(UserNameToSendMessage))
                Debug.WriteLine("UserNameToSendMessage not configured in app.config! Some tests may fail.");

            NumberToGetUserFull = ConfigurationManager.AppSettings[nameof(NumberToGetUserFull)];
            if (string.IsNullOrEmpty(NumberToGetUserFull))
                Debug.WriteLine("NumberToGetUserFull not configured in app.config! Some tests may fail.");

            NumberToAddToChat = ConfigurationManager.AppSettings[nameof(NumberToAddToChat)];
            if (string.IsNullOrEmpty(NumberToAddToChat))
                Debug.WriteLine("NumberToAddToChat not configured in app.config! Some tests may fail.");
        }

        [TestMethod]
        public async Task AuthUser()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, ApiId, ApiHash);

            await client.Connect();

            var codeRequest = await client.SendCodeRequest(NumberToAuthenticate);
            var hash = codeRequest.phoneCodeHash;
            var code = ""; // you can change code in debugger
            Debugger.Break();

            var user = await client.MakeAuth(NumberToAuthenticate, hash, code);

            Assert.IsNotNull(user);
            Assert.IsTrue(client.IsUserAuthorized());

            await client.Close();
        }

        [TestMethod]
        public async Task SignUpNewUser()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, ApiId, ApiHash);
            await client.Connect();

            var codeRequest = await client.SendCodeRequest(NotRegisteredNumberToSignUp);
            var hash = codeRequest.phoneCodeHash;
            var code = "";

            var registeredUser = await client.SignUp(NotRegisteredNumberToSignUp, hash, code, "Telegram.Net", "User");
            Assert.IsNotNull(registeredUser);
            Assert.IsTrue(client.IsUserAuthorized());

            var loggedInUser = await client.MakeAuth(NotRegisteredNumberToSignUp, hash, code);
            Assert.IsNotNull(loggedInUser);
        }

        [TestMethod]
        public async Task CheckPhones()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, ApiId, ApiHash);
            await client.Connect();

            var result = await client.IsPhoneRegistered(NumberToAuthenticate);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ImportContactByPhoneNumber()
        {
            var client = await InitializeClient();

            var contact = await client.ImportContactByPhoneNumber(NumberToSendMessage, "Contact", "Contact");
            Assert.IsNotNull(contact);
        }

        [TestMethod]
        public async Task ResolveUsername()
        {
            var client = await InitializeClient();

            var user = await client.ResolveUsername(UserNameToSendMessage);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public async Task ImportByUserNameAndSendMessage()
        {
            var client = await InitializeClient();

            var user = await client.ResolveUsername(UserNameToSendMessage);
            Assert.IsNotNull(user);

            var contactUser = user as UserContactConstructor;
            Assert.IsNotNull(contactUser);

            await client.SendDirectMessage(contactUser.id, "Test message from TelegramClient");
        }

        [TestMethod]
        public async Task ImportContactByPhoneNumberAndSendMessage()
        {
            var client = await InitializeClient();
            var userId = await ImportAndGetUserId(client, NumberToSendMessage);

            await client.SendDirectMessage(userId, "Test message from TelegramClient");
        }

        [TestMethod]
        public async Task GetHistory()
        {
            var client = await InitializeClient();
            var userId = await ImportAndGetUserId(client, NumberToSendMessage);

            var hist = await client.GetMessagesHistoryForContact(userId, 0, 5);
            Assert.IsNotNull(hist);
        }

        [TestMethod]
        public async Task UploadAndSendMedia()
        {
            var client = await InitializeClient();
            var userId = await ImportAndGetUserId(client, NumberToSendMessage);

            var file = File.ReadAllBytes("../../data/cat.jpg");
            var mediaFile = await client.UploadFile("test_file.jpg", file);
            Assert.IsNotNull(mediaFile);

            var state = await client.SendMediaMessage(userId, mediaFile);
            Assert.IsTrue(state);
        }

        [TestMethod]
        public async Task GetFile()
        {
            // Get uploaded file from last message (ie: cat.jpg)

            var client = await InitializeClient();
            var user = await client.ImportContactByPhoneNumber(NumberToSendMessage, "Contact", "Contact");

            // Get last message
            var hist = await client.GetMessagesHistoryForContact(user.user_id, 0, 1);
            Assert.AreEqual(1, hist.Count);

            var message = (MessageConstructor) hist[0];
            Assert.AreEqual(typeof (MessageMediaPhotoConstructor), message.media.GetType());

            var media = (MessageMediaPhotoConstructor) message.media;
            Assert.AreEqual(typeof (PhotoConstructor), media.photo.GetType());

            var photo = (PhotoConstructor) media.photo;
            Assert.AreEqual(3, photo.sizes.Count);
            Assert.AreEqual(typeof (PhotoSizeConstructor), photo.sizes[2].GetType());

            var photoSize = (PhotoSizeConstructor) photo.sizes[2];
            Assert.AreEqual(typeof (FileLocationConstructor), photoSize.location.GetType());

            var fileLocation = (FileLocationConstructor) photoSize.location;
            var file = await client.GetFile(fileLocation.volume_id, fileLocation.local_id, fileLocation.secret, 0, photoSize.size + 1024);

            storage_FileType type = file.Item1;
            byte[] bytes = file.Item2;

            string name = "../../data/get_file.";
            if (type.GetType() == typeof (Storage_fileJpegConstructor))
                name += "jpg";
            else if (type.GetType() == typeof (Storage_fileGifConstructor))
                name += "gif";
            else if (type.GetType() == typeof (Storage_filePngConstructor))
                name += "png";

            using (var fileStream = new FileStream(name, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(bytes, 4, photoSize.size); // The first 4 bytes seem to be the error code
            }
        }

        [TestMethod]
        public async Task TestConnection()
        {
            var store = new FakeSessionStore();
            var client = new TelegramClient(store, ApiId, ApiHash);

            await client.Connect();
        }

        [TestMethod]
        public async Task AuthenticationWorks()
        {
            using (var transport = new TcpTransport("91.108.56.165", 443))
            {
                var authKey = await Authenticator.DoAuthentication(transport);

                Assert.IsNotNull(authKey.AuthKey.Data);
            }
        }

        [TestMethod]
        public async Task GetUserFullRequest()
        {
            var client = await InitializeClient();

            var userId = await ImportAndGetUserId(client, NumberToSendMessage);
            var userFull = await client.GetUserFull(userId);

            Assert.IsNotNull(userFull);
        }

        [TestMethod]
        public async Task CreateChatRequest()
        {
            var client = await InitializeClient();

            var chatName = Guid.NewGuid().ToString();
            var userIdToInvite = await ImportAndGetUserId(client, NumberToSendMessage);
            var statedMessage = await client.CreateChat(chatName, new List<int> { userIdToInvite });

            var createdChat = GetChatFromStatedMessage(statedMessage);

            Assert.AreEqual(chatName, createdChat.title);
            Assert.AreEqual(2, createdChat.participants_count);
        }

        [TestMethod]
        public async Task AddChatUserRequest()
        {
            var client = await InitializeClient();

            var chatName = Guid.NewGuid().ToString();
            var userIdToInvite = await ImportAndGetUserId(client, NumberToSendMessage);
            var statedMessageAfterCreation = await client.CreateChat(chatName, new List<int> { userIdToInvite });

            var createdChat = GetChatFromStatedMessage(statedMessageAfterCreation);

            var userIdToAdd = await ImportAndGetUserId(client, NumberToSendMessage);

            var statedMessageAfterAddUser = await client.AddChatUser(createdChat.id, userIdToAdd);
            var modifiedChat = GetChatFromStatedMessage(statedMessageAfterAddUser);

            Assert.AreEqual(createdChat.id, modifiedChat.id);
            Assert.AreEqual(3, modifiedChat.participants_count);
        }

        [TestMethod]
        public async Task LeaveChatRequest()
        {
            var client = await InitializeClient();

            var chatName = Guid.NewGuid().ToString();
            var userIdToInvite = await ImportAndGetUserId(client, NumberToSendMessage);
            var statedMessageAfterCreation = await client.CreateChat(chatName, new List<int> { userIdToInvite });

            var createdChat = GetChatFromStatedMessage(statedMessageAfterCreation);
            
            var statedMessageAfterLeave = await client.LeaveChat(createdChat.id);
            var modifiedChat = GetChatFromStatedMessage(statedMessageAfterLeave);

            Assert.AreEqual(createdChat.id, modifiedChat.id);
            Assert.AreEqual(1, modifiedChat.participants_count);
        }

        [TestMethod]
        public async Task GetUpdates()
        {
            var client = await InitializeClient();

            var updatesState = await client.GetUpdatesState();
            var initialState = updatesState as Updates_stateConstructor;

            Assert.IsNotNull(initialState);

            var difference = await client.GetUpdatesDifference(initialState.pts, initialState.date, initialState.qts);
            Assert.IsNotNull(difference);
            Assert.AreEqual(difference.Constructor, Constructor.updates_differenceEmpty);

            var userIdToSendMessage = await ImportAndGetUserId(client, NumberToSendMessage);
            await client.SendDirectMessage(userIdToSendMessage, "test");

            var differenceAfterMessage = await client.GetUpdatesDifference(initialState.pts, initialState.date, initialState.qts);

            Assert.IsNotNull(differenceAfterMessage);
            Assert.AreEqual(differenceAfterMessage.Constructor, Constructor.updates_difference);

            var differenceUpdate = differenceAfterMessage as Updates_differenceConstructor;
            Assert.IsNotNull(differenceUpdate);
            Assert.AreEqual(1, differenceUpdate.new_messages.Count);

            var messageUpdate = differenceUpdate.new_messages[0] as MessageConstructor;
            Assert.IsNotNull(messageUpdate);

            Assert.AreEqual("test", messageUpdate.message);
        }

        [TestMethod]
        public async Task DataCenterMigrationErrorHandling()
        {
            var phoneForDc5 = GetTestPhoneOfDc(5);

            var store = new FakeSessionStore();
            var client = new TelegramClient(store, ApiId, ApiHash);
            await client.Connect();

            var codeRequest = await client.SendCodeRequest(phoneForDc5);
            Assert.IsFalse(string.IsNullOrEmpty(codeRequest.phoneCodeHash));
        }

        /*[TestMethod]
        public async Task UpdatesHandling()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", ApiId, ApiHash);
            await client.Connect();

            Assert.IsTrue(client.IsUserAuthorized());

            var userId = await client.ImportContactByPhoneNumber(NumberToSendMessage);

            var waiter = new UpdatesWaiter(client);
            var updateTask = waiter.WaitNext();

            var req = new SendMessageRequest(new InputPeerContactConstructor(userId.Value), "bullshit");
            await client.Send(req);

            var upd = await updateTask;
        }*/

        private ChatConstructor GetChatFromStatedMessage(Messages_statedMessageConstructor message)
        {
            var serviceMessage = message.message as MessageServiceConstructor;
            Assert.IsNotNull(serviceMessage);

            var peerChat = serviceMessage.to_id as PeerChatConstructor;
            Assert.IsNotNull(peerChat);

            var createdChatId = peerChat.chat_id;
            return message.chats.OfType<ChatConstructor>().Single(c => c.id == createdChatId);
        }

        private async Task<TelegramClient> InitializeClient()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, ApiId, ApiHash);
            await client.Connect();

            if (!client.IsUserAuthorized())
            {
                var codeRequest = await client.SendCodeRequest(NumberToAuthenticate);
                var hash = codeRequest.phoneCodeHash;

                var code = ""; // you can change code in debugger
                Debugger.Break();

                await client.MakeAuth(NumberToAuthenticate, hash, code);
            }

            Assert.IsTrue(client.IsUserAuthorized());

            return client;
        }

        private async Task<int> ImportAndGetUserId(TelegramClient client, string phoneNumber)
        {
            var user = await client.ImportContactByPhoneNumber(phoneNumber, phoneNumber, "Contact");
            Assert.IsNotNull(user);

            return user.user_id;
        }

        private string GetTestPhoneOfDc(int dcNumber)
        {
            return $"99966{dcNumber}0000"; // 99966XYYYY : x-dcId; yyyy-random numbers
        }

        class UpdatesWaiter
        {
            private TaskCompletionSource<Updates> current = new TaskCompletionSource<Updates>();

            public UpdatesWaiter(TelegramClient client)
            {
                client.UpdateMessage += ConnectionUpdateMessage;
            }

            private void ConnectionUpdateMessage(object sender, Updates update)
            {
                current.SetResult(update);
                current = new TaskCompletionSource<Updates>();
            }

            public Task<Updates> WaitNext()
            {
                return current.Task;
            }
        }
    }
}
