using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class ResolveUsernameRequest : MtProtoRequest
    {
        private readonly string username;
        public User user { get; private set; }

        public ResolveUsernameRequest(string username)
        {
            this.username = username;
        }

        protected override uint requestCode => 0xbf0131c;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            Serializers.String.Write(writer, username);
        }

        public override void OnResponse(BinaryReader reader)
        {
            user = TLObject.Read<User>(reader);
        }

        public override bool isContentMessage => true;
    }
}