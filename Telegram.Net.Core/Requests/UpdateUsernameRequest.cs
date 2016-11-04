using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto;
namespace Telegram.Net.Core.Requests
{
    public class UpdateUsernameRequest : MTProtoRequest
    {
        private readonly string userName;

        public User UserResponse { get; private set; }

        protected override uint requestCode => 0x3e0bdd7c;

        public UpdateUsernameRequest(string userName)
        {
            this.userName = userName;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            Serializers.String.Write(writer, userName);
        }

        public override void OnResponse(BinaryReader reader)
        {
            UserResponse = TLObject.Read<User>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
