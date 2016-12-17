using System.IO;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Requests
{
    public class SendMessageRequest : MtProtoRequest
    {
        public readonly InputPeer peer;
        public readonly string message;
        public readonly long randomId;

        public SentMessage sentMessage { get; private set; }

        public SendMessageRequest(InputPeer peer, string message)
        {
            this.peer = peer;
            this.message = message;

            randomId = Helpers.GenerateRandomLong();
        }

        protected override uint requestCode => 0x4cde0aab;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            peer.Write(writer);
            Serializers.String.Write(writer, message);
            writer.Write(randomId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            sentMessage = TLObject.Read<SentMessage>(reader);
        }

        public override bool isContentMessage => true;
    }
}
