using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class ResolveUsernameRequest : MTProtoRequest
    {
        private readonly string userName;
        public User User { get; private set; }

        public ResolveUsernameRequest(string userName)
        {
            this.userName = userName;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0xBF0131C);
            Serializers.String.Write(writer, userName);
        }

        public override void OnResponse(BinaryReader reader)
        {
            User = TL.Parse<User>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}