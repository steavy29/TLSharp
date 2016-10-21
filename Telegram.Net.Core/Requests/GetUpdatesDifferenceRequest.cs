using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetUpdatesDifferenceRequest : MTProtoRequest
    {
        public readonly int pts;
        public readonly int date;
        public readonly int qts;

        public GetUpdatesDifferenceRequest(int pts, int date, int qts)
        {
            this.pts = pts;
            this.date = date;
            this.qts = qts;
        }

        public UpdatesDifference updatesDifference { get; private set; }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0xa041495);
            writer.Write(pts);
            writer.Write(date);
            writer.Write(qts);
        }

        public override void OnResponse(BinaryReader reader)
        {
            updatesDifference = TL.Parse<UpdatesDifference>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
