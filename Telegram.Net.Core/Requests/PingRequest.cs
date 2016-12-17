using System.IO;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Requests
{
    public class PingRequest : MtProtoRequest
    {
        public long pingId;

        public PingRequest()
        {
            pingId = Helpers.GenerateRandomLong();
        }

        protected override uint requestCode => 0x7abe77ec;
        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            writer.Write(pingId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            pingId = reader.ReadInt64();
        }

        public override bool isContentMessage => true;
    }
}
