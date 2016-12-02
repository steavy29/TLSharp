using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetDialogsRequest : MtProtoRequest
    {
        private readonly int offset;
        private readonly int maxId;
        private readonly int limit;

        public MessagesDialogs messagesDialogs { get; private set; }

        public GetDialogsRequest(int offset, int maxId, int limit)
        {
            this.offset = offset;
            this.maxId = maxId;
            this.limit = limit;
        }

        protected override uint requestCode => 0xeccf1df6;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            writer.Write(offset);
            writer.Write(maxId);
            writer.Write(limit);
        }

        public override void OnResponse(BinaryReader reader)
        {
            messagesDialogs = TLObject.Read<MessagesDialogs>(reader);
        }

        public override bool isContentMessage => true;
    }
}
