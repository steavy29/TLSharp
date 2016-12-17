using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AddChatUserRequest : MtProtoRequest
    {
        public readonly int chatId;
        public readonly InputUser user;
        public readonly int messagesToForward;

        public MessagesStatedMessage statedMessage { get; private set; }

        public AddChatUserRequest(int chatId, InputUser user, int messagesToForward)
        {
            this.chatId = chatId;
            this.user = user;
            this.messagesToForward = messagesToForward;
        }

        protected override uint requestCode => 0x2ee9ee9e;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            writer.Write(chatId);
            user.Write(writer);
            writer.Write(messagesToForward);
        }

        public override void OnResponse(BinaryReader reader)
        {
            statedMessage = TLObject.Read<MessagesStatedMessageConstructor>(reader);
        }

        public override bool isContentMessage => true;
    }
}
