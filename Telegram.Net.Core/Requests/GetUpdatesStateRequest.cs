using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetUpdatesStateRequest : MtProtoRequest
    {
        public UpdatesState updatesState { get; private set; }

        protected override uint requestCode => 0xedd4882a;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
        }

        public override void OnResponse(BinaryReader reader)
        {
            updatesState = TLObject.Read<UpdatesState>(reader);
        }

        public override bool isContentMessage => true;
    }
}