using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class AuthSendSmsRequest : MTProtoRequest
    {
        private readonly string _phoneNumber;
        private readonly string _phoneCodeHash;
        public bool _smsSent;
        
        public AuthSendSmsRequest(string phoneNumber, string phoneCodeHash)
        {
            _phoneNumber = phoneNumber;
            _phoneCodeHash = phoneCodeHash;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0xda9f3e8);
            Serializers.String.Write(writer, _phoneNumber);
            Serializers.String.Write(writer, _phoneCodeHash);
        }

        public override void OnResponse(BinaryReader reader)
        {
            var dataCode = reader.ReadUInt32();
            _smsSent = reader.ReadUInt32() == 0x997275b5;
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
