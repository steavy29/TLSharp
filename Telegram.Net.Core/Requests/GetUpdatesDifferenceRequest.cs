using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetUpdatesDifferenceRequest : MtProtoRequest
    {
        public readonly int pts;
        public readonly int date;
        public readonly int qts;

        public UpdatesDifference updatesDifference { get; private set; }

        public GetUpdatesDifferenceRequest(int pts, int date, int qts)
        {
            this.pts = pts;
            this.date = date;
            this.qts = qts;
        }

        protected override uint requestCode => 0xa041495;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            writer.Write(pts);
            writer.Write(date);
            writer.Write(qts);
        }

        public override void OnResponse(BinaryReader reader)
        {
            updatesDifference = TLObject.Read<UpdatesDifference>(reader);
        }

        public override bool isContentMessage => true;
    }
}
