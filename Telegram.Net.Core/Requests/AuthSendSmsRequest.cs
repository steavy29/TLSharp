using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSendSmsRequest : MtProtoRequest
    {
        private readonly string phoneNumber;
        private readonly string phoneCodeHash;

        public bool smsSent { get; private set; }
        
        public AuthSendSmsRequest(string phoneNumber, string phoneCodeHash)
        {
            this.phoneNumber = phoneNumber;
            this.phoneCodeHash = phoneCodeHash;
        }

        protected override uint requestCode => 0xda9f3e8;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            Serializers.String.Write(writer, phoneNumber);
            Serializers.String.Write(writer, phoneCodeHash);
        }

        public override void OnResponse(BinaryReader reader)
        {
            smsSent = TLObject.ReadBool(reader);
        }

        public override bool isContentMessage => true;
    }
}
