using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Requests
{
    public class PingRequest : MtProtoRequest
    {
        public long pingId;

        public PingRequest()
        {
            pingId = Helpers.GenerateRandomLong();
        }

        protected override uint requestCode => 0x7abe77ec;
        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            writer.Write(pingId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            pingId = reader.ReadInt64();
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool isContentMessage => true;
        public override bool Responded { get; }
    }
}
