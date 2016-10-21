using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSendCodeRequest : MTProtoRequest
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

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0x768d5f4d);
            Serializers.String.Write(writer, phoneNumber);
            writer.Write(smsType);
            writer.Write(apiId);
            Serializers.String.Write(writer, apiHash);
            Serializers.String.Write(writer, langCode);
        }

        public override void OnResponse(BinaryReader reader)
        {
            sentCode = TL.Parse<AuthSentCode>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
