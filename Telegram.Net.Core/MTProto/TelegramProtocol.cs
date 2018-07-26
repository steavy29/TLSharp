using System;
using System.Collections.Generic;
using System.IO;

namespace Telegram.Net.Core.MTProto
{
    public partial class TelegramProtocol
    {
        //public static int Read(BinaryReader reader)
        //{
        //    reader.Read<T>()()
        //}

        public static void Write<T>(T obj, BinaryWriter writer) where T: TLObject
        {
            obj.Write(writer);
        }

        public static void Write(int obj, BinaryWriter writer)
        {
            writer.Write(obj);
        }

        public static void Write(double obj, BinaryWriter writer)
        {
            writer.Write(obj);
        }

        public static void Write(float obj, BinaryWriter writer)
        {
            writer.Write(obj);
        }

        public static void Write<T>(List<T> obj, BinaryWriter writer, Action<T, BinaryWriter> writeElementFunc)
        {
            writer.Write(Codes.VectorCode);
            writer.Write(obj.Count);
            foreach (var element in obj)
            {
                writeElementFunc(element, writer);
            }
        }
    }
}