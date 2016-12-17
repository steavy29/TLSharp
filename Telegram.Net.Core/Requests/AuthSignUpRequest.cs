using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSignUpRequest : MtProtoRequest
    {
        public readonly string phoneNumber;
        public readonly string phoneCodeHash;
        public readonly string code;
        public readonly string firstName;
        public readonly string lastName;

        public AuthAuthorization authorization { get; private set; }

        public AuthSignUpRequest(string phoneNumber, string phoneCodeHash, string code, string firstName, string lastName)
        {
            this.phoneNumber = phoneNumber;
            this.phoneCodeHash = phoneCodeHash;
            this.code = code;
            this.firstName = firstName;
            this.lastName = lastName; 
        }

        protected override uint requestCode => 0x1b067634;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            Serializers.String.Write(writer, phoneNumber);
            Serializers.String.Write(writer, phoneCodeHash);
            Serializers.String.Write(writer, code);
            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
        }

        public override void OnResponse(BinaryReader reader)
        {
            authorization = TLObject.Read<AuthAuthorization>(reader);
        }

        public override bool isContentMessage => true;
    }
}
