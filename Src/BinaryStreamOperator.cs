using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartache
{
    public class BinaryStreamOperator
    {
        public static string ReadChunkTag(BinaryReader reader, ref int readByte)
        {
            return ReadCharArrayAsString(reader, 4, ref readByte);
        }

        public static string ReadCharArrayAsString(BinaryReader reader, int length, ref int readByte)
        {
            readByte += length;
            return ReadCharArrayAsString(reader, length);
        }

        public static string ReadChunkTag(BinaryReader reader)
        {
            return ReadCharArrayAsString(reader, 4);
        }

        public static string ReadCharArrayAsString(BinaryReader reader, int length)
        {
            char[] chunkArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                chunkArray[i] = reader.ReadChar();
            }
            return new string(chunkArray);
        }

        public static string ReadPascalString(BinaryReader reader, ref int readByte)
        {
            int stringLength = ReadSize(reader, ref readByte);
            return ReadCharArrayAsString(reader, stringLength, ref readByte);
        }

        public static string ReadPascalString(BinaryReader reader)
        {
            int stringLength = ReadSize(reader);
            return ReadCharArrayAsString(reader, stringLength);
        }


        public static int ReadSize(BinaryReader reader, ref int readByte)
        {
            readByte += 4;
            return ReadSize(reader);
        }

        public static int ReadSize(BinaryReader reader)
        {
            return reader.ReadInt32();
        }

        public static Int16 ReadInt16(BinaryReader reader)
        {
            return reader.ReadInt16();
        }


        public static int ReadPosition(BinaryReader reader, ref int readByte)
        {
            readByte += 4;
            return ReadPosition(reader);
        }

        public static int ReadPosition(BinaryReader reader)
        {
            return reader.ReadInt32();
        }

        public static byte[] ReadBinary(BinaryReader reader, int size)
        {
            return reader.ReadBytes(size);
        }

        public static byte[] ReadBinary(BinaryReader reader, int size, ref int readByte)
        {
            readByte += size;
            return ReadBinary(reader, size);
        }
    }
}
