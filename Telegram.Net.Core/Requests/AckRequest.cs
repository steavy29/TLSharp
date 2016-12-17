using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AckRequestLong : MtProtoRequest
    {
        private readonly List<long> messageIds;

        public AckRequestLong(List<long> messageIds)
        {
            this.messageIds = messageIds;
        }

        protected override uint requestCode => 0x62d6b459;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            TLObject.WriteVector(writer, messageIds, writer.Write);
        }

        public override void OnResponse(BinaryReader reader) { }
        
        public override bool isContentMessage => false;
    }
}
