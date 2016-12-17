using System.IO;
using Telegram.Net.Core;

namespace Telegram.Net.Tests
{
    public class FileSessionStore : ISessionStore
    {
        private readonly string sessionFileName;

        public FileSessionStore(string sessionFileName = "session.dat")
        {
            this.sessionFileName = sessionFileName;
        }

        public void Save(Session session)
        {
            using (var stream = new FileStream(sessionFileName, FileMode.OpenOrCreate))
            {
                var result = session.ToBytes();
                stream.Write(result, 0, result.Length);
            }
        }

        public Session Load()
        {
            if (!File.Exists(sessionFileName))
                return null;

            using (var stream = new FileStream(sessionFileName, FileMode.Open))
            {
                var buffer = new byte[2048];
                stream.Read(buffer, 0, 2048);

                return Session.FromBytes(buffer, this);
            }
        }
    }
}
