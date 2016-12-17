using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Telegram.Net.Core.MTProto;

namespace Telegram.Net.Core.Requests
{
    public class GetContactsRequest : MtProtoRequest
    {
        public readonly string contactIdsHash = "";

        public ContactsContacts contacts { get; private set; }

        public GetContactsRequest(IEnumerable<int> currentContacts = null)
        {
            if (currentContacts != null)
            {
                var joinedSortedIds = string.Join(",", currentContacts.OrderBy(i => i));
                contactIdsHash = string.Concat(MTProto.Crypto.MD5.GetMd5Bytes(Encoding.UTF8.GetBytes(joinedSortedIds)).Select(b => b.ToString("x2")));
            }
        }

        protected override uint requestCode => 0x22c6aa08;

        public override void OnSend(BinaryWriter writer)
        {
            writer.Write(requestCode);
            Serializers.String.Write(writer, contactIdsHash);
        }

        public override void OnResponse(BinaryReader reader)
        {
            contacts = TLObject.Read<ContactsContacts>(reader);
        }

        public override bool isContentMessage => true;
    }
}
