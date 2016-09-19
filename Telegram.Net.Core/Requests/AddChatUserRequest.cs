using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    class AddChatUserRequest : MTProtoRequest
    {
        private readonly int chatId;
        private readonly InputUserContactConstructor user;

        public Messages_statedMessageConstructor message;

        public AddChatUserRequest(int chatId, InputUserContactConstructor user)
        {
            this.chatId = chatId;
            this.user = user;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0x2ee9ee9e);
            writer.Write(chatId);
            user.Write(writer);
            writer.Write(1);
        }

        public override void OnResponse(BinaryReader reader)
        {
            message = TL.Parse<Messages_statedMessageConstructor>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed { get { return true; } }
        public override bool Responded { get; }
    }
}
