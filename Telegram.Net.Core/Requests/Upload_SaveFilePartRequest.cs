using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    //upload.saveFilePart#b304a621 file_id:long file_part:int bytes:bytes = Bool;
    public class Upload_SaveFilePartRequest : MTProtoRequest
    {
        private readonly long fileId;
        private readonly int filePart;
        private readonly byte[] data;
        private readonly int count;

        public bool Done { get; set; }

        public Upload_SaveFilePartRequest(long fileId, int filePart, byte[] data, int count)
        {
            this.fileId = fileId;
            this.filePart = filePart;
            this.data = data;
            this.count = count;
        }

        public override void OnResponse(BinaryReader reader)
        {
            var code = reader.ReadUInt32();

            if (code != 0xbc799737 && code != 0x997275b5)
                throw new InvalidOperationException($"Expected Tl Bool type");

            Done = code == 0x997275b5 ? true : false;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0xb304a621);
            writer.Write(fileId);
            writer.Write(filePart);

            Serializers.Bytes.Write(writer, data, count);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
