using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class SaveFilePartRequest : MtProtoRequest
    {
        public readonly long fileId;
        public readonly int filePart;
        public readonly byte[] bytesBuffer;
        public readonly int offset;
        public readonly int count;

        public bool done { get; private set; }

        public SaveFilePartRequest(long fileId, int filePart, byte[] bytesBuffer, int offset, int count)
        {
            this.fileId = fileId;
            this.filePart = filePart;
            this.bytesBuffer = bytesBuffer;
            this.offset = offset;
            this.count = count;
        }

        protected override uint requestCode => 0xb304a621;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            writer.Write(fileId);
            writer.Write(filePart);
            Serializers.Bytes.Write(writer, bytesBuffer, offset, count);
        }

        public override void OnResponse(BinaryReader reader)
        {
            done = TLObject.ReadBool(reader);
        }

        public override bool isContentMessage => true;
    }
}
