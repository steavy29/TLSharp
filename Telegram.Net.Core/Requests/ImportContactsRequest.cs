using System.Collections.Generic;
using System.IO;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class ImportContactsRequest : MtProtoRequest
    {
        private readonly List<InputContact> contacts;
        private readonly bool replace;

        public ContactsImportedContacts importedContacts { get; private set; }

        public ImportContactsRequest(List<InputContact> contacts, bool replace)
        {
            this.contacts = contacts;
            this.replace = replace;
        }

        protected override uint requestCode => 0xda30b32d;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);

            TLObject.WriteVector(writer, contacts);
            TLObject.WriteBool(writer, replace);
        }

        public override void OnResponse(BinaryReader reader)
        {
            importedContacts = TLObject.Read<ContactsImportedContacts>(reader);
        }

        public override bool isContentMessage => true;
    }
}
