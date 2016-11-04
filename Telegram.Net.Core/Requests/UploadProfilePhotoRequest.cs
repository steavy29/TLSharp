using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class UploadProfilePhotoRequest : MTProtoRequest
    {
        private readonly InputFile file;

        public Photo photo { get; private set; }

        protected override uint requestCode => 0x4f32c098;

        public UploadProfilePhotoRequest(InputFile file)
        {
            this.file = file;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            file.Write(writer);
        }

        public override void OnResponse(BinaryReader reader)
        {
            photo = TLObject.Read<Photo>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
