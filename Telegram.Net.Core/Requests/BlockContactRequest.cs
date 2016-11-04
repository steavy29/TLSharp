using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    class BlockContactRequest : MTProtoRequest
    {
        private readonly InputUser user;

        public bool state { get; private set; }

        protected override uint requestCode => 0x332b49fc;

        public BlockContactRequest(InputUser user)
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
            state = TLObject.Read<bool>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
