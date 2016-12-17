using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetUsersRequest : MtProtoRequest
    {
        public readonly List<InputUser> id;

        public List<User> users { get; private set; }

        public GetUsersRequest(List<InputUser> id)
        {
            this.id = id;
        }

        protected override uint requestCode => 0xd91a548;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            TLObject.WriteVector(writer, users);
        }

        public override void OnResponse(BinaryReader reader)
        {
            users = TLObject.ReadVector<User>(reader);
        }

        public override bool isContentMessage => true;
    }
}
