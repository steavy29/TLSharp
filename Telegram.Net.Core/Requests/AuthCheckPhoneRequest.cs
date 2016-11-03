using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthCheckPhoneRequest : MTProtoRequest
    {
        private readonly string phoneNumber;

        public AuthCheckedPhone checkedPhone { get; private set; }

        public AuthCheckPhoneRequest(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        protected override uint requestCode => 0x6fe51dfb;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            Serializers.String.Write(writer, phoneNumber);
        }

        public override void OnResponse(BinaryReader reader)
        {
            checkedPhone = TLObject.Read<AuthCheckedPhone>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
