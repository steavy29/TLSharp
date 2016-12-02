using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSendCallRequest : MtProtoRequest
    {
        private readonly string phoneNumber;
        private readonly string phoneCodeHash;

        public bool callSent { get; private set; }

        public AuthSendCallRequest(string phoneNumber, string phoneCodeHash)
        {
            this.phoneNumber = phoneNumber;
            this.phoneCodeHash = phoneCodeHash;
        }

        protected override uint requestCode => 0x3c51564;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            Serializers.String.Write(writer, phoneNumber);
            Serializers.String.Write(writer, phoneCodeHash);
        }

        public override void OnResponse(BinaryReader reader)
        {
            callSent = TLObject.ReadBool(reader);
        }
        
        public override bool isContentMessage => true;
    }
}