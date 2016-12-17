using System.IO;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Requests
{
    public class SendMediaRequest : MtProtoRequest
    {
        public readonly InputPeer inputPeer;
        public readonly InputMedia inputMedia;
        public readonly long randomId;

        public MessagesStatedMessage statedMessage { get; private set; }

        public SendMediaRequest(InputPeer inputPeer, InputMedia inputMedia)
        {
            this.inputPeer = inputPeer;
            this.inputMedia = inputMedia;

            randomId = Helpers.GenerateRandomLong();
        }

        protected override uint requestCode => 0xa3c85d76;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            inputPeer.Write(writer);
            inputMedia.Write(writer);
            writer.Write(randomId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            statedMessage = TLObject.Read<MessagesStatedMessage>(reader);
        }

        public override bool isContentMessage => true;
    }
}
