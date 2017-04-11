using System;
using System.IO;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.MTProto.Crypto;
using Telegram.Net.Core.Requests;

#pragma warning disable 675

namespace Telegram.Net.Core
{
    public interface ISessionStore
    {
        void Save(Session session);
        Session Load();
    }

    public class Session
    {
        public string serverAddress;
        public int port;
        public AuthKey authKey;
        public ulong id;
        public int sequence;
        public ulong salt;
        public int timeOffset;
        public long lastMessageId;
        public int sessionExpires;
        public User user;
        private readonly Random random;

        private readonly ISessionStore store;

        private readonly object generateSyncRoot = new object();

        private Session(ISessionStore store)
        {
            random = new Random();
            this.store = store;
        }

        public byte[] ToBytes()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(id);
                writer.Write(sequence);
                writer.Write(salt);
                writer.Write(lastMessageId);
                writer.Write(timeOffset);
                Serializers.String.Write(writer, serverAddress);
                writer.Write(port);

                if (user != null)
                {
                    writer.Write(1);
                    writer.Write(sessionExpires);
                    user.Write(writer);
                }
                else
                {
                    writer.Write(0);
                }

                if (authKey?.Data != null)
                {
                    Serializers.Bytes.Write(writer, authKey.Data);
                }

                return stream.ToArray();
            }
        }

        public static Session FromBytes(byte[] buffer, ISessionStore store)
        {
            using (var stream = new MemoryStream(buffer))
            using (var reader = new BinaryReader(stream))
            {
                var id = reader.ReadUInt64();
                var sequence = reader.ReadInt32();
                var salt = reader.ReadUInt64();
                var lastMessageId = reader.ReadInt64();
                var timeOffset = reader.ReadInt32();
                var serverAddress = Serializers.String.Read(reader);
                var port = reader.ReadInt32();

                var isAuthExsist = reader.ReadInt32() == 1;
                int sessionExpires = 0;
                User user = null;
                if (isAuthExsist)
                {
                    sessionExpires = reader.ReadInt32();
                    user = TLObject.Read<User>(reader);
                }

                var authData = Serializers.Bytes.Read(reader);

                return new Session(store)
                {
                    authKey = new AuthKey(authData),
                    id = id,
                    salt = salt,
                    sequence = sequence,
                    lastMessageId = lastMessageId,
                    timeOffset = timeOffset,
                    sessionExpires = sessionExpires,
                    user = user,
                    serverAddress = serverAddress,
                    port = port
                };
            }
        }

        public void Save()
        {
            store?.Save(this);
        }

        public static Session TryLoadOrCreateNew(string serverAddress, int port, ISessionStore store = null)
        {
            return store?.Load() ?? new Session(store)
            {
                id = GenerateRandomUlong(),
                serverAddress = serverAddress,
                port = port
            };
        }

        private static ulong GenerateRandomUlong()
        {
            var random = new Random();
            ulong rand = (((ulong)random.Next()) << 32) | ((ulong)random.Next());
            return rand;
        }

        public long GetNewMessageId()
        {
            lock (generateSyncRoot)
            {
                long time = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
                long newMessageId = ((time / 1000 + timeOffset) << 32) |
                                    ((time % 1000) << 22) |
                                    (random.Next(524288) << 2); // 2^19
                                                                // [ unix timestamp : 32 bit] [ milliseconds : 10 bit ] [ buffer space : 1 bit ] [ random : 19 bit ] [ msg_id type : 2 bit ] = [ msg_id : 64 bit ]

                if (lastMessageId >= newMessageId)
                {
                    newMessageId = lastMessageId + 4;
                }

                lastMessageId = newMessageId;
                return newMessageId;
            }
        }

        public int GetNextSequenceNumber(MtProtoRequest request)
        {
            lock (generateSyncRoot)
            {
                return request.isContentMessage ? sequence++ * 2 + 1 : sequence * 2;
            }
        }

        public void ResetAuth()
        {
            lock (generateSyncRoot)
            {
                authKey = null;
                user = null;
                sessionExpires = 0;
            }
        }

        public void Reset()
        {
            lock (generateSyncRoot)
            {
                id = GenerateRandomUlong();
                lastMessageId = 0;
                sequence = 0;
                timeOffset = 0;
            }
        }
    }
}
