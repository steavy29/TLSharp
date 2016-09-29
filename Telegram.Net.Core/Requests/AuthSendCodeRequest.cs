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
        public bool phoneRegistered;
        public string phoneCodeHash;

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
            Serializers.String.write(writer, phoneNumber);
            writer.Write(smsType);
            writer.Write(apiId);
            Serializers.String.write(writer, apiHash);
            Serializers.String.write(writer, langCode);
        }

        public override void OnResponse(BinaryReader reader)
        {
            var boolTrue = 0x997275b5;
            var dataCode = reader.ReadUInt32();

            var phoneRegisteredValue = reader.ReadUInt32();
            phoneRegistered = phoneRegisteredValue == boolTrue;

            phoneCodeHash = Serializers.String.read(reader);

            var sendCodeTimeout = reader.ReadInt32();
            var isPasswordValue = reader.ReadUInt32();
            var isPassword = isPasswordValue == boolTrue;
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
