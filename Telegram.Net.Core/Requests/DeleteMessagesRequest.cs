using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class DeleteMessagesRequest : MtProtoRequest
    {
        public readonly List<int> messageIdsToDelete;
        public List<int> deletedMessageIds { get; private set; }

        public DeleteMessagesRequest(List<int> messageIdsToDelete)
        {
            this.messageIdsToDelete = messageIdsToDelete;
        }

        protected override uint requestCode => 0x14f2dd0a;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            TLObject.WriteVector(writer, messageIdsToDelete, writer.Write);
        }

        public override void OnResponse(BinaryReader reader)
        {
            deletedMessageIds = TLObject.ReadVector(reader, reader.ReadInt32);
        }

        public override bool isContentMessage => true;
    }
}