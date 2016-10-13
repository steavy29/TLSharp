using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    class CreateChatRequest : MTProtoRequest
    {
        private readonly List<InputUser> inputUsers;
        private readonly string title;

        public Messages_statedMessageConstructor message;
        public CreateChatRequest(List<InputUser> inputUsers, string title)
        {
            this.inputUsers = inputUsers;
            this.title = title;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0x419d9aee);
            writer.Write(0x1cb5c415); // vector#1cb5c415
            writer.Write(inputUsers.Count); // vector length
            foreach (var id in inputUsers)
                id.Write(writer);
            Serializers.String.Write(writer, title);
        }

        public override void OnResponse(BinaryReader reader)
        {
            message = TL.Parse<Messages_statedMessageConstructor>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed { get { return true; } }
        public override bool Responded { get; }
    }
}
