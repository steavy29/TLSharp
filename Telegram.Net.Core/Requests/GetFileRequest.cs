using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetFileRequest : MtProtoRequest
    {
        public readonly InputFileLocation location;
        public readonly int offset;
        public readonly int limit;

        public UploadFile file { get; private set; }

        public GetFileRequest(InputFileLocation location, int offset, int limit)
        {
            this.location = location;
            this.offset = offset;
            this.limit = limit;
        }

        protected override uint requestCode => 0xe3a6cfb5;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            location.Write(writer);
            writer.Write(offset);
            writer.Write(limit);
        }

        public override void OnResponse(BinaryReader reader)
        {
            file = TLObject.Read<UploadFile>(reader);
        }

        public override bool isContentMessage => true;
    }
}
