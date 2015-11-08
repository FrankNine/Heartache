using System;
using System.Text;

using NUnit.Framework;
using System.IO;

namespace Heartache.Test
{
    class BinaryStreamOperatorTests
    {

        [Test]
        public void ReadChunkTag_Match()
        {
            const string tag = "FORM";

            using (BinaryWriter writer = _CreateWriter())
            {
                _WriteString(writer, tag);

                BinaryReader reader = new BinaryReader(writer.BaseStream);
                reader.BaseStream.Position = 0;

                Assert.AreEqual(tag, BinaryStreamOperator.ReadChunkTag(reader));
            }
        }

        BinaryWriter _CreateWriter()
        {
            MemoryStream stream = new MemoryStream();
            return new BinaryWriter(stream, Encoding.ASCII);
        }

        void _WriteSize(BinaryWriter writer, int size)
        {
            writer.Write((Int32)size);
        }

        void _WriteString(BinaryWriter writer, string stringToWrite)
        {
            writer.Write(Encoding.ASCII.GetBytes(stringToWrite));
        }

        void _WritePascalString(BinaryWriter writer, string stringToWrite)
        {
            _WriteSize(writer, stringToWrite.Length);
            _WriteString(writer, stringToWrite);
        }

        void _WriteFuckedString(BinaryWriter writer, string stringToWrite)
        {
            _WritePascalString(writer, stringToWrite);
            writer.Write('\0');
        }

        [Test]
        public void ReadCunkTag_Match_SizeMatch()
        {
            const string tag = "FORM";
            const int startingPosition = 5;
            int position = startingPosition;

            using (BinaryWriter writer = _CreateWriter())
            {
                _WriteString(writer, tag);

                BinaryReader reader = new BinaryReader(writer.BaseStream);
                reader.BaseStream.Position = 0;

                string readTag = BinaryStreamOperator.ReadChunkTag(reader, ref position);
                Assert.AreEqual(tag, readTag);
                Assert.AreEqual(startingPosition + tag.Length, position);
            }
        }

        [Test]
        public void ReadPascalString_Match()
        {
            const string stringToWrite = "How'd do you turn this on";

            using (BinaryWriter writer = _CreateWriter())
            {
                _WritePascalString(writer, stringToWrite);

                BinaryReader reader = new BinaryReader(writer.BaseStream);
                reader.BaseStream.Position = 0;

                string readString = BinaryStreamOperator.ReadPascalString(reader);
                Assert.AreEqual(stringToWrite, readString);
            }
        }

        [Test]
        public void ReadPascalString_Match_SizeMatch()
        {
            const string stringToWrite = "How'd do you turn this on";
            const int startingPosition = 13;
            int position = startingPosition;

            using (BinaryWriter writer = _CreateWriter())
            {
                _WritePascalString(writer, stringToWrite);

                BinaryReader reader = new BinaryReader(writer.BaseStream);
                reader.BaseStream.Position = 0;

                string readString = BinaryStreamOperator.ReadPascalString(reader, ref position);
                Assert.AreEqual(stringToWrite, readString);
                Assert.AreEqual(startingPosition + sizeof(Int32) + stringToWrite.Length , position);
            }
        }
    }
}
