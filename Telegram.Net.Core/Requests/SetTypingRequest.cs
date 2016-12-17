using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class SetTypingRequest : MtProtoRequest
    {
        private readonly InputPeer peer;
        private readonly SendMessageAction action;

        public bool state { get; private set; }

        protected override uint requestCode => 0xa3825e50;

        public SetTypingRequest(InputPeer peer, SendMessageAction action)
        {
            this.peer = peer;
            this.action = action;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            peer.Write(writer);
            writer.Write((uint)action);
        }

        public override void OnResponse(BinaryReader reader)
        {
            state = TLObject.Read<bool>(reader);
        }

        public override bool isContentMessage => true;
    }
}
