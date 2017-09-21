using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class ReceivedMessagesRequest : MtProtoRequest
    {
        private readonly int maxId;

        public List<int> PushNotificationsCanceledForIds { get; private set; }

        public ReceivedMessagesRequest(int maxId)
        {
            this.maxId = maxId;
        }

        protected override uint requestCode => 0x28abcb68;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            writer.Write(maxId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            PushNotificationsCanceledForIds = TLObject.ReadVector(reader, reader.ReadInt32);
        }

        public override bool isContentMessage => true;
    }
}
