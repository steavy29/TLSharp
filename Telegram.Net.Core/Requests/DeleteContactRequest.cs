using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    class DeleteContactRequest : MTProtoRequest
    {
        private readonly InputUser user;

        public ContactsLink link { get; private set; }

        protected override uint requestCode => 0x8e953744;

        public DeleteContactRequest(InputUser user)
        {
            this.user = user;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            user.Write(writer);
        }

        public override void OnResponse(BinaryReader reader)
        {
            link = TLObject.Read<ContactsLink>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
