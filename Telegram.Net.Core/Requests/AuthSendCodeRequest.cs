using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSendCodeRequest : MtProtoRequest
    {
        private readonly string phoneNumber;
        private readonly int smsType;
        private readonly int apiId;
        private readonly string apiHash;
        private readonly string langCode;

        public AuthSentCode sentCode { get; private set; }

        public AuthSendCodeRequest(string phoneNumber, int smsType, int apiId, string apiHash, string langCode)
        {
            this.phoneNumber = phoneNumber;
            this.smsType = smsType;
            this.apiId = apiId;
            this.apiHash = apiHash;
            this.langCode = langCode;
        }

        protected override uint requestCode => 0x768d5f4d;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            Serializers.String.Write(writer, phoneNumber);
            writer.Write(smsType);
            writer.Write(apiId);
            Serializers.String.Write(writer, apiHash);
            Serializers.String.Write(writer, langCode);
        }

        public override void OnResponse(BinaryReader reader)
        {
            sentCode = TLObject.Read<AuthSentCode>(reader);
        }

        public override bool isContentMessage => true;
    }
}
