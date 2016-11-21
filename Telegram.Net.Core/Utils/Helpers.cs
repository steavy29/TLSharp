using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Telegram.Net.Core.MTProto.Crypto;
using Telegram.Net.Core.Requests;

#pragma warning disable 675

namespace Telegram.Net.Core.Utils
{
    public static class Helpers
    {
        private static readonly Random random = new Random();

        public static ulong GenerateRandomUlong()
        {
            ulong rand = (((ulong)random.Next()) << 32) | ((ulong)random.Next());
            return rand;
        }

        public static long GenerateRandomLong()
        {
            long rand = (((long)random.Next()) << 32) | ((long)random.Next());
            return rand;
        }

        public static byte[] GenerateRandomBytes(int num)
        {
            byte[] data = new byte[num];
            random.NextBytes(data);
            return data;
        }

        public static AESKeyData CalcKey(byte[] sharedKey, byte[] msgKey, bool client)
        {
            int x = client ? 0 : 8;
            byte[] buffer = new byte[48];

            Array.Copy(msgKey, 0, buffer, 0, 16);            // buffer[0:16] = msgKey
            Array.Copy(sharedKey, x, buffer, 16, 32);     // buffer[16:48] = authKey[x:x+32]
            byte[] sha1A = GetSha1(buffer);                     // sha1a = GetSha1(buffer)

            Array.Copy(sharedKey, 32 + x, buffer, 0, 16);   // buffer[0:16] = authKey[x+32:x+48]
            Array.Copy(msgKey, 0, buffer, 16, 16);           // buffer[16:32] = msgKey
            Array.Copy(sharedKey, 48 + x, buffer, 32, 16);  // buffer[32:48] = authKey[x+48:x+64]
            byte[] sha1B = GetSha1(buffer);                     // sha1b = GetSha1(buffer)

            Array.Copy(sharedKey, 64 + x, buffer, 0, 32);   // buffer[0:32] = authKey[x+64:x+96]
            Array.Copy(msgKey, 0, buffer, 32, 16);           // buffer[32:48] = msgKey
            byte[] sha1C = GetSha1(buffer);                     // sha1c = GetSha1(buffer)

            Array.Copy(msgKey, 0, buffer, 0, 16);            // buffer[0:16] = msgKey
            Array.Copy(sharedKey, 96 + x, buffer, 16, 32);  // buffer[16:48] = authKey[x+96:x+128]
            byte[] sha1D = GetSha1(buffer);                     // sha1d = GetSha1(buffer)

            byte[] key = new byte[32];                       // key = sha1a[0:8] + sha1b[8:20] + sha1c[4:16]
            Array.Copy(sha1A, 0, key, 0, 8);
            Array.Copy(sha1B, 8, key, 8, 12);
            Array.Copy(sha1C, 4, key, 20, 12);

            byte[] iv = new byte[32];                        // iv = sha1a[8:20] + sha1b[0:8] + sha1c[16:20] + sha1d[0:8]
            Array.Copy(sha1A, 8, iv, 0, 12);
            Array.Copy(sha1B, 0, iv, 12, 8);
            Array.Copy(sha1C, 16, iv, 20, 4);
            Array.Copy(sha1D, 0, iv, 24, 8);

            return new AESKeyData(key, iv);
        }

        public static byte[] CalcMsgKey(byte[] data)
        {
            byte[] msgKey = new byte[16];
            Array.Copy(GetSha1(data), 4, msgKey, 0, 16);
            return msgKey;
        }

        public static byte[] CalcMsgKey(byte[] data, int offset, int limit)
        {
            byte[] msgKey = new byte[16];
            Array.Copy(GetSha1(data, offset, limit), 4, msgKey, 0, 16);
            return msgKey;
        }

        public static byte[] GetSha1(byte[] data)
        {
            using (SHA1 sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(data);
            }
        }

        public static byte[] GetSha1(byte[] data, int offset, int limit)
        {
            using (SHA1 sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(data, offset, limit);
            }
        }

        public static MemoryStream CreateMemoryStream(int len)
        {
            return new MemoryStream(new byte[len], 0, len, true, true);
        }

        public static byte[] Serialize(this MtProtoRequest request)
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new BinaryWriter(memoryStream))
            {
                request.OnSend(writer);

                return memoryStream.ToArray();
            }
        }

        public static void IgnoreAwait(this Task task)
        {
        }
    }
}
