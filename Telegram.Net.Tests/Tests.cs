﻿using System;
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
using Telegram.Net.Core.Requests;

namespace Telegram.Net.Tests
{
    [TestClass]
    public class Tests
    {
        private int apiId => int.Parse(ConfigurationManager.AppSettings[nameof(apiId)]);
        private string apiHash => ConfigurationManager.AppSettings[nameof(apiHash)];
        private string numberToSendMessage => ConfigurationManager.AppSettings[nameof(numberToAuthenticate)];
        private string numberToAuthenticate => ConfigurationManager.AppSettings[nameof(notRegisteredNumberToSignUp)];
        private string notRegisteredNumberToSignUp => ConfigurationManager.AppSettings[nameof(numberToSendMessage)];
        private string userNameToSendMessage => ConfigurationManager.AppSettings[nameof(userNameToSendMessage)];
        private string numberToGetUserFull => ConfigurationManager.AppSettings[nameof(numberToGetUserFull)];
        private string numberToAddToChat => ConfigurationManager.AppSettings[nameof(numberToAddToChat)];

        [TestMethod]
        public async Task AuthUser()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, apiId, apiHash);

            await client.Connect();

            var codeRequest = await client.SendCode(numberToAuthenticate, VerificationCodeDeliveryType.NumericCodeViaTelegram);
            var hash = codeRequest.phoneCodeHash;
            var code = ""; // you can change code in debugger
            Debugger.Break();

            var user = await client.SignIn(numberToAuthenticate, hash, code);

            Assert.IsNotNull(user);
            Assert.IsTrue(client.IsUserAuthorized());

            await client.Close();
        }

        [TestMethod]
        public async Task SignUpNewUser()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, apiId, apiHash);
            await client.Connect();

            var codeRequest = await client.SendCode(notRegisteredNumberToSignUp, VerificationCodeDeliveryType.NumericCodeViaTelegram);
            var hash = codeRequest.phoneCodeHash;
            var code = "";

            var registeredUser = await client.SignUp(notRegisteredNumberToSignUp, hash, code, "Telegram.Net", "User");
            Assert.IsNotNull(registeredUser);
            Assert.IsTrue(client.IsUserAuthorized());

            var loggedInUser = await client.SignIn(notRegisteredNumberToSignUp, hash, code);
            Assert.IsNotNull(loggedInUser);
        }

        [TestMethod]
        public async Task CheckPhones()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, apiId, apiHash);
            await client.Connect();

            var result = await client.CheckPhone(numberToAuthenticate);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.phoneRegistered);
        }

        [TestMethod]
        public async Task ImportContactByPhoneNumber()
        {
            var client = await InitializeClient();

            var importedConacts = await client.ImportContactByPhoneNumber(numberToSendMessage, "Contact", "Contact");

            Assert.IsNotNull(importedConacts);
            Assert.AreEqual(1, importedConacts.importedContacts.Count);
            Assert.AreEqual(1, importedConacts.users.Count);
        }

        [TestMethod]
        public async Task ResolveUsername()
        {
            var client = await InitializeClient();

            var resolveUsernameRequest = new ResolveUsernameRequest(userNameToSendMessage);
            await client.SendRpcRequest(resolveUsernameRequest);

            Assert.IsNotNull(resolveUsernameRequest.user);
        }

        [TestMethod]
        public async Task ImportByUserNameAndSendMessage()
        {
            var client = await InitializeClient();

            var resolveUsernameRequest = new ResolveUsernameRequest(userNameToSendMessage);
            await client.SendRpcRequest(resolveUsernameRequest);

            var contactUser = resolveUsernameRequest.user as UserContactConstructor;
            Assert.IsNotNull(contactUser);

            await client.SendDirectMessage(contactUser.id, "Test message from TelegramClient");
        }

        [TestMethod]
        public async Task ImportContactByPhoneNumberAndSendMessage()
        {
            var client = await InitializeClient();
            var user = await ImportAndGetUser(client, numberToSendMessage);

            await client.SendDirectMessage(user.userId, "Test message from TelegramClient");
        }

        [TestMethod]
        public async Task GetHistory()
        {
            var client = await InitializeClient();
            var user = await ImportAndGetUser(client, numberToSendMessage);

            var hist = await client.GetHistoryForContact(user.userId, 0, 5);
            Assert.IsNotNull(hist);
        }

        [TestMethod]
        public async Task UploadAndSendMedia()
        {
            var client = await InitializeClient();
            var user = await ImportAndGetUser(client, numberToSendMessage);

            InputFile mediaFile;
            using (var fileStream = File.OpenRead("../../data/cat.jpg"))
            {
                mediaFile = await client.UploadFile("test_file.jpg", fileStream);
            }
            Assert.IsNotNull(mediaFile);

            var state = await client.SendMediaMessage(new InputPeerContactConstructor(user.userId), new InputMediaUploadedPhotoConstructor(mediaFile));
            Assert.IsNotNull(state);
        }

        [TestMethod]
        public async Task GetFile()
        {
            // Get uploaded file from last message (ie: cat.jpg)
            var client = await InitializeClient();

            var user = await ImportAndGetUser(client, numberToSendMessage);

            // Get last message
            var hist = ((MessagesMessagesConstructor)await client.GetHistoryForContact(user.userId, 0, 1)).messages;
            Assert.AreEqual(1, hist.Count);

            var message = (MessageConstructor)hist[0];
            Assert.AreEqual(typeof(MessageMediaPhotoConstructor), message.media.GetType());

            var media = (MessageMediaPhotoConstructor)message.media;
            Assert.AreEqual(typeof(PhotoConstructor), media.photo.GetType());

            var photo = (PhotoConstructor)media.photo;
            Assert.AreEqual(3, photo.sizes.Count);
            Assert.AreEqual(typeof(PhotoSizeConstructor), photo.sizes[2].GetType());

            var photoSize = (PhotoSizeConstructor)photo.sizes[2];
            Assert.AreEqual(typeof(FileLocationConstructor), photoSize.location.GetType());

            var fileLocation = (FileLocationConstructor)photoSize.location;
            var file = await client.GetFile(fileLocation.volumeId, fileLocation.localId, fileLocation.secret, 0, photoSize.size + 1024);

            string name = "../../data/get_file.";
            if (file.type.GetType() == typeof(StorageFileJpegConstructor))
                name += "jpg";
            else if (file.type.GetType() == typeof(StorageFileGifConstructor))
                name += "gif";
            else if (file.type.GetType() == typeof(StorageFilePngConstructor))
                name += "png";

            using (var fileStream = new FileStream(name, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(file.bytes, 4, photoSize.size); // The first 4 bytes seem to be the error code
            }
        }

        [TestMethod]
        public async Task TestConnection()
        {
            var store = new FakeSessionStore();
            var client = new TelegramClient(store, apiId, apiHash);

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

            var user = await ImportAndGetUser(client, numberToSendMessage);

            var getUserFullRequest = new GetFullUserRequest(new InputUserContactConstructor(user.userId));
            await client.SendRpcRequest(getUserFullRequest);

            Assert.IsNotNull(getUserFullRequest.userFull);
        }

        [TestMethod]
        public async Task CreateChatRequest()
        {
            var client = await InitializeClient();

            var chatName = Guid.NewGuid().ToString();
            var userToInvite = await ImportAndGetUser(client, numberToSendMessage);
            var statedMessage = await client.CreateChat(chatName, new List<InputUser> { new InputUserContactConstructor(userToInvite.userId) });

            var createdChat = GetChatFromStatedMessage(statedMessage);

            Assert.AreEqual(chatName, createdChat.title);
            Assert.AreEqual(2, createdChat.participantsCount);
        }

        [TestMethod]
        public async Task AddChatUserRequest()
        {
            var client = await InitializeClient();

            var chatName = Guid.NewGuid().ToString();
            var userToInvite = await ImportAndGetUser(client, numberToSendMessage);
            var statedMessageAfterCreation = await client.CreateChat(chatName, new List<InputUser> { new InputUserContactConstructor(userToInvite.userId) });

            var createdChat = GetChatFromStatedMessage(statedMessageAfterCreation);

            var userToAdd = await ImportAndGetUser(client, numberToSendMessage);

            var statedMessageAfterAddUser = await client.AddChatUser(createdChat.id, new InputUserContactConstructor(userToAdd.userId), 0);
            var modifiedChat = GetChatFromStatedMessage(statedMessageAfterAddUser);

            Assert.AreEqual(createdChat.id, modifiedChat.id);
            Assert.AreEqual(3, modifiedChat.participantsCount);
        }

        [TestMethod]
        public async Task LeaveChatRequest()
        {
            var client = await InitializeClient();

            var chatName = Guid.NewGuid().ToString();
            var userToInvite = await ImportAndGetUser(client, numberToSendMessage);
            var statedMessageAfterCreation = await client.CreateChat(chatName, new List<InputUser> { new InputUserContactConstructor(userToInvite.userId) });

            var createdChat = GetChatFromStatedMessage(statedMessageAfterCreation);

            var statedMessageAfterLeave = await client.LeaveChat(createdChat.id);
            var modifiedChat = GetChatFromStatedMessage(statedMessageAfterLeave);

            Assert.AreEqual(createdChat.id, modifiedChat.id);
            Assert.AreEqual(1, modifiedChat.participantsCount);
        }

        [TestMethod]
        public async Task GetUpdates()
        {
            var client = await InitializeClient();

            var updatesStateRequest = new GetUpdatesStateRequest();
            await client.SendRpcRequest(updatesStateRequest);
            var initialState = (UpdatesStateConstructor)updatesStateRequest.updatesState;

            var request = new GetUpdatesDifferenceRequest(initialState.pts, initialState.date, initialState.qts);
            await client.SendRpcRequest(request);
            Assert.IsNotNull(request.updatesDifference);
            Assert.AreEqual(request.updatesDifference.constructor, Constructor.UpdatesDifferenceEmpty);

            var importedUser = await ImportAndGetUser(client, numberToSendMessage);
            await client.SendDirectMessage(importedUser.userId, "test");

            var differenceRequestAfterMessage = new GetUpdatesDifferenceRequest(initialState.pts, initialState.date, initialState.qts);
            await client.SendRpcRequest(differenceRequestAfterMessage);

            Assert.IsNotNull(differenceRequestAfterMessage.updatesDifference);
            Assert.AreEqual(differenceRequestAfterMessage.updatesDifference.constructor, Constructor.UpdatesDifference);

            var differenceUpdate = (UpdatesDifferenceConstructor)differenceRequestAfterMessage.updatesDifference;
            Assert.AreEqual(1, differenceUpdate.newMessages.Count);

            var messageUpdate = (MessageConstructor)differenceUpdate.newMessages[0];
            Assert.AreEqual("test", messageUpdate.message);
        }

        [TestMethod]
        public async Task DataCenterMigrationErrorHandling()
        {
            var phoneForDc5 = GetTestPhoneOfDc(5);

            var store = new FakeSessionStore();
            var client = new TelegramClient(store, apiId, apiHash);
            await client.Connect();

            var codeRequest = await client.SendCode(phoneForDc5, VerificationCodeDeliveryType.NumericCodeViaTelegram);
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

        private ChatConstructor GetChatFromStatedMessage(MessagesStatedMessage message)
        {
            var statedMessage = message.Cast<MessagesStatedMessageConstructor>();
            var createdChatId = statedMessage.message.Cast<MessageServiceConstructor>().toId.Cast<PeerChatConstructor>().chatId;
            return statedMessage.chats.OfType<ChatConstructor>().Single(c => c.id == createdChatId);
        }

        private async Task<TelegramClient> InitializeClient()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, apiId, apiHash);
            await client.Connect();

            if (!client.IsUserAuthorized())
            {
                var codeRequest = await client.SendCode(numberToAuthenticate, VerificationCodeDeliveryType.NumericCodeViaTelegram);
                var hash = codeRequest.phoneCodeHash;

                var code = ""; // you can change code in debugger
                Debugger.Break();

                await client.SignIn(numberToAuthenticate, hash, code);
            }

            Assert.IsTrue(client.IsUserAuthorized());

            return client;
        }

        private async Task<ImportedContactConstructor> ImportAndGetUser(TelegramClient client, string phoneNumber)
        {
            var contacts = await client.ImportContactByPhoneNumber(phoneNumber, phoneNumber, "Contact");

            Assert.IsNotNull(contacts);
            Assert.AreEqual(1, contacts.importedContacts.Count);

            return (ImportedContactConstructor)contacts.importedContacts[0];
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
