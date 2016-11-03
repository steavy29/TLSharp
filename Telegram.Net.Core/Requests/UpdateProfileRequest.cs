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
        private readonly string firstName;
        private readonly string lastName;
        private int flags { get; set; }

        public User UserResponse { get; private set; }

        protected override uint requestCode => 0x78515775;

        public void ComputeFlags(string firstName, string lastName)
        {
            flags = 0;
            flags = firstName != null ? (flags | 1) : (flags & ~1);
            flags = lastName != null ? (flags | 2) : (flags & ~2);
        }

        public UpdateProfileRequest(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            ComputeFlags(firstName, lastName);
            writer.Write(flags);

            Serializers.String.Write(writer, firstName);
            Serializers.String.Write(writer, lastName);
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
