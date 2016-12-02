using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class CreateChatRequest : MtProtoRequest
    {
        public readonly List<InputUser> inputUsers;
        public readonly string title;

        public MessagesStatedMessage statedMessage { get; private set; }

        public CreateChatRequest(List<InputUser> inputUsers, string title)
        {
            this.inputUsers = inputUsers;
            this.title = title;
        }

        protected override uint requestCode => 0x419d9aee;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            TLObject.WriteVector(writer, inputUsers);
            Serializers.String.Write(writer, title);
        }

        public override void OnResponse(BinaryReader reader)
        {
            statedMessage = TLObject.Read<MessagesStatedMessage>(reader);
        }

        public override bool isContentMessage => true;
    }
}
