using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthExportAuthorizationRequest : MtProtoRequest
    {
        private readonly int dcId;
        public AuthExportedAuthorization exportedAuthorization { get; private set; }

        protected override uint requestCode => 0xe5bfffcd;

        public AuthExportAuthorizationRequest(int dcId)
        {
            this.dcId = dcId;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            writer.Write(dcId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            exportedAuthorization = TLObject.Read<AuthExportedAuthorization>(reader);
        }

        public override bool isContentMessage => true;
    }
}