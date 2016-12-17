using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetFullUserRequest : MtProtoRequest
    {
        public readonly InputUser inputUser;

        public UserFull userFull { get; private set; }

        public GetFullUserRequest(InputUser inputUser)
        {
            this.inputUser = inputUser;
        }

        protected override uint requestCode => 0xca30a5b1;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            inputUser.Write(writer);
        }

        public override void OnResponse(BinaryReader reader)
        {
            userFull = TLObject.Read<UserFull>(reader);
        }

        public override bool isContentMessage => true;
    }
}
