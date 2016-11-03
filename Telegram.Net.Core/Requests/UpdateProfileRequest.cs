using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class UpdateProfileRequest : MTProtoRequest
    {
        private readonly string firstname;
        private readonly string lastname;
        private int flags { get; set; }

        public User UserResponse { get; private set; }

        protected override uint requestCode => 0x78515775;

        public void ComputeFlags(string firstname, string lastname)
        {
            flags = 0;
            flags = first_name != null ? (flags | 1) : (flags & ~1);
            flags = last_name != null ? (flags | 2) : (flags & ~2);
        }

        public UpdateProfileRequest(string firstname, string lastname)
        {
            this.firstname = firstname;
            this.lastname = lastname;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            ComputeFlags(first_name, last_name);
            writer.Write(flags);

            Serializers.String.Write(writer, first_name);
            Serializers.String.Write(writer, last_name);
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
