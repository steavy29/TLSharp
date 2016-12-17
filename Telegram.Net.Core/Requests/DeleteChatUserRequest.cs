using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class DeleteChatUserRequest : MtProtoRequest
    {
        public readonly int chatId;
        public readonly InputUser user;

        public MessagesStatedMessage statedMessage { get; private set; }

        public DeleteChatUserRequest(int chatId, InputUser user)
        {
            this.chatId = chatId;
            this.user = user;
        }

        protected override uint requestCode => 0xc3c5cd23;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            writer.Write(chatId);
            user.Write(writer);
        }

        public override void OnResponse(BinaryReader reader)
        {
            statedMessage = TLObject.Read<MessagesStatedMessage>(reader);
        }

        public override bool isContentMessage => true;
    }
}
