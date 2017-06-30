using System.IO;

using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetFullChatRequest : MtProtoRequest
    {
        public readonly int chatId;

        public ChatFull chatFull { get; private set; }

        public GetFullChatRequest(int chatId)
        {
            this.chatId = chatId;
        }

        protected override uint requestCode => 0xe5d7d19c;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            writer.Write(chatId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            chatFull = TLObject.Read<ChatFull>(reader);
        }
    }
}