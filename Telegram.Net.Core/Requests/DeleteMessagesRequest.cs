using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    class DeleteMessagesRequest : MTProtoRequest
    {
        private readonly List<int> messageIdsToDelete;
        public List<int> deletedMessageIds;

        public DeleteMessagesRequest(List<int> messageIdsToDelete)
        {
            this.messageIdsToDelete = messageIdsToDelete;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0x14f2dd0a);

            writer.Write(0x1cb5c415); //vector
            writer.Write(messageIdsToDelete.Count);
            foreach (int messageId in messageIdsToDelete)
            {
                writer.Write(messageId);
            }
        }

        public override void OnResponse(BinaryReader reader)
        {
            deletedMessageIds = TL.ParseVector(reader, reader.ReadInt32);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}