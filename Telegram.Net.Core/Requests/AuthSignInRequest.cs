using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSignInRequest : MtProtoRequest
    {
        private readonly string phoneNumber;
        private readonly string phoneCodeHash;
        private readonly string code;

        public AuthAuthorization authorization { get; private set; }

        public AuthSignInRequest(string phoneNumber, string phoneCodeHash, string code)
        {
            this.phoneNumber = phoneNumber;
            this.phoneCodeHash = phoneCodeHash;
            this.code = code;
        }

        protected override uint requestCode => 0xbcd51581;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            Serializers.String.Write(writer, phoneNumber);
            Serializers.String.Write(writer, phoneCodeHash);
            Serializers.String.Write(writer, code);
        }

        public override void OnResponse(BinaryReader reader)
        {
            authorization = TLObject.Read<AuthAuthorization>(reader);
        }

        public override bool isContentMessage => true;
    }
}
