using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetNearestDcRequest : MtProtoRequest
    {
        public NearestDc nearestDc { get; private set; }

        protected override uint requestCode => 0x1fb33026;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
        }

        public override void OnResponse(BinaryReader reader)
        {
            nearestDc = TLObject.Read<NearestDc>(reader);
        }

        public override bool isContentMessage => true;
    }
}
