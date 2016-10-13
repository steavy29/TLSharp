using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthCheckPhoneRequest : MTProtoRequest
    {
        private readonly string phoneNumber;
        public auth_CheckedPhone checkedPhone;

        public AuthCheckPhoneRequest(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0x6fe51dfb);
            Serializers.String.Write(writer, phoneNumber);
        }

        public override void OnResponse(BinaryReader reader)
        {
            checkedPhone = TL.Parse<auth_CheckedPhone>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
