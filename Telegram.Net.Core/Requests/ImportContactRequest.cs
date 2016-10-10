using System;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class ImportContactRequest : MTProtoRequest
    {
        private readonly InputContact contact;
        private readonly bool replace;

        public contacts_ImportedContacts importedContacts;

        public ImportContactRequest(InputContact contact, bool replace = true)
        {
            this.contact = contact;
            this.replace = replace;
        }

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(0xda30b32d);
            writer.Write(0x1cb5c415);
            writer.Write(1);
            contact.Write(writer);
            writer.Write(replace ? 0x997275b5 : 0xbc799737);
        }

        public override void OnResponse(BinaryReader reader)
        {
            importedContacts = TL.Parse<contacts_ImportedContacts>(reader);
        }

        public override void OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override bool Confirmed => true;
        public override bool Responded { get; }
    }
}
