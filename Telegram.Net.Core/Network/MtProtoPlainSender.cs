using System;
using System.IO;
using System.Threading.Tasks;
#pragma warning disable 675

namespace Telegram.Net.Core.Network
{
    public class MtProtoPlainSender
    {
        //private int sequence = 0;
#pragma warning disable 649 // TODO: check if we can remove it
        private int timeOffset;
#pragma warning restore 649
        private long lastMessageId;
        private readonly Random random;
        private readonly TcpTransport transport;

        public MtProtoPlainSender(TcpTransport transport)
        {
            this.transport = transport;
            random = new Random();
        }

        public async Task Send(byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write((long)0);
                    binaryWriter.Write(GetNewMessageId());
                    binaryWriter.Write(data.Length);
                    binaryWriter.Write(data);

                    byte[] packet = memoryStream.ToArray();

                    await transport.Send(packet);
                }
            }
        }

        public async Task<byte[]> Receive()
        {
            var result = await transport.Receieve();

            using (var memoryStream = new MemoryStream(result.body))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    binaryReader.ReadInt64(); // authKeyid
                    binaryReader.ReadInt64(); // messageId
                    int messageLength = binaryReader.ReadInt32();

                    byte[] response = binaryReader.ReadBytes(messageLength);

                    return response;
                }
            }
        }

        private long GetNewMessageId()
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
}
