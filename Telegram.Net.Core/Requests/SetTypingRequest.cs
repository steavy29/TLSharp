using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class SetTypingRequest : MTProtoRequest
    {
        private readonly InputPeer peer;
        private readonly int action;

        public bool state { get; private set; }

        protected override uint requestCode => 0xa3825e50;

        public SetTypingRequest(InputPeer peer)
        {
            this.peer = peer;
            this.action = 0x16bf744e;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            peer.Write(writer);
            writer.Write(action);
        }

        public override void OnResponse(BinaryReader reader)
        {
            state = TLObject.Read<bool>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Responded => true;
        public override bool Confirmed => true;
    }
}
