using System;
using System.IO;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.Utils;

namespace Telegram.Net.Core.Requests
{
    public class SendMessageRequest : MTProtoRequest
    {
        private readonly InputPeer peer;
        private readonly string message;

        public SentMessage sentMessage { get; private set; }

        public SendMessageRequest(InputPeer peer, string message)
        {
            this.peer = peer;
            this.message = message;
        }

        public override void OnSend(BinaryWriter writer)
        {
            long randomId = Helpers.GenerateRandomLong();
            writer.Write(0x4cde0aab);
            peer.Write(writer);
            Serializers.String.Write(writer, message);
            writer.Write(randomId);
        }

        public override void OnResponse(BinaryReader reader)
        {
            sentMessage = TL.Parse<SentMessage>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
