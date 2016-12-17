using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetHistoryRequest : MtProtoRequest
    {
        public readonly InputPeer peer;
        public readonly int offset;
        public readonly int maxId;
        public readonly int limit;

        public MessagesMessages messages;

        public GetHistoryRequest(InputPeer peer, int offset, int maxId, int limit)
        {
            this.peer = peer;
            this.offset = offset;
            this.maxId = maxId;
            this.limit = limit;
        }

        protected override uint requestCode => 0x92a1df2f;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            peer.Write(writer);
            writer.Write(offset);
            writer.Write(maxId);
            writer.Write(limit);
        }

        public override void OnResponse(BinaryReader reader)
        {
            messages = TLObject.Read<MessagesMessages>(reader);
        }

        public override bool isContentMessage => true;
    }
}
