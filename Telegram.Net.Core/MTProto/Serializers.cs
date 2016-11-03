using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Telegram.Net.Core.MTProto
{
    public class Serializers
    {
        public static class Bytes
        {
            public static byte[] Read(BinaryReader binaryReader)
            {
                byte firstByte = binaryReader.ReadByte();
                int len, padding;
                if (firstByte == 254)
                {
                    len = binaryReader.ReadByte() | (binaryReader.ReadByte() << 8) | (binaryReader.ReadByte() << 16);
                    padding = len % 4;
                }
                else {
                    len = firstByte;
                    padding = (len + 1) % 4;
                }

                byte[] data = binaryReader.ReadBytes(len);
                if (padding > 0)
                {
                    padding = 4 - padding;
                    binaryReader.ReadBytes(padding);
                }

                return data;
            }

            public static BinaryWriter Write(BinaryWriter binaryWriter, byte[] buffer, int offset, int count)
            {
                int padding;
                if (count < 254)
                {
                    padding = (count + 1) % 4;
                    if (padding != 0)
                    {
                        padding = 4 - padding;
                    }

                    binaryWriter.Write((byte)count);
                    binaryWriter.Write(buffer, offset, count);
                }
                else
                {
                    padding = count % 4;
                    if (padding != 0)
                    {
                        padding = 4 - padding;
                    }

                    binaryWriter.Write((byte)254);
                    binaryWriter.Write((byte)count);
                    binaryWriter.Write((byte)(count >> 8));
                    binaryWriter.Write((byte)(count >> 16));
                    binaryWriter.Write(buffer, offset, count);
                }


                for (int i = 0; i < padding; i++)
                {
                    binaryWriter.Write((byte)0);
                }

                return binaryWriter;
            }

            public static BinaryWriter Write(BinaryWriter binaryWriter, byte[] data)
            {
                return Write(binaryWriter, data, 0, data.Length);
            }
        }

        public static class String
        {
            public static string Read(BinaryReader reader)
            {
                byte[] data = Bytes.Read(reader);
                return Encoding.UTF8.GetString(data, 0, data.Length);
            }

            public static BinaryWriter Write(BinaryWriter writer, string str)
            {
                return Bytes.Write(writer, Encoding.UTF8.GetBytes(str));
            }
        }

        public static string VectorToString<T>(List<T> list)
        {
            string[] tokens = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                tokens[i] = list[i].ToString();
            }
            return "[" + System.String.Join(", ", tokens) + "]";
        }
    }
}
