using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetConfigRequest: MtProtoRequest
    {
        protected override uint requestCode => 0xc4f9186b;

        public Config config { get; private set; }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
        }

        public override void OnResponse(BinaryReader reader)
        {
            config = TLObject.Read<Config>(reader);
        }
    }
}
