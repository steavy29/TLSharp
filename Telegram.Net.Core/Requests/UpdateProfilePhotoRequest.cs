using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class UpdateProfilePhotoRequest : MTProtoRequest
    {
        private readonly InputPhoto id;

        public UserProfilePhoto photo { get; private set; }

        protected override uint requestCode => 0xf0bb5152;

        public UpdateProfilePhotoRequest(InputPhoto id)
        {
            this.id = id;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            id.Write(writer);
        }

        public override void OnResponse(BinaryReader reader)
        {
            photo = TLObject.Read<UserProfilePhoto>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
