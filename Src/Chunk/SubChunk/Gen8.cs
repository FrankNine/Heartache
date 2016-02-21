using System.IO;
using System.Text;

using Newtonsoft.Json;

using Heartache.Primitive;

namespace Heartache.Chunk
{
    class Gen8 : Chunk
    {
        public const string TAG = "GEN8";
        private const string INDEX_FILENAME = "Index.txt";

        class Data
        {
            public byte[] unknown1;

            public StringEntry string1 = new StringEntry();
            public StringEntry string2 = new StringEntry();

            public byte[] unknown2;

            public StringEntry string3 = new StringEntry();

            public byte[] unknown3;

            public int width;
            public int height;

            public byte[] unknown4;

            public StringEntry string4 = new StringEntry();

            public byte[] unknown5;
        }

        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);

            _data.unknown1 = BinaryStreamOperator.ReadBinary(reader, 4);
            _data.string1.position = BinaryStreamOperator.ReadPosition(reader);
            _data.string2.position = BinaryStreamOperator.ReadPosition(reader);
            _data.unknown2 = BinaryStreamOperator.ReadBinary(reader, 28);
            _data.string3.position = BinaryStreamOperator.ReadPosition(reader);
            _data.unknown3 = BinaryStreamOperator.ReadBinary(reader, 16);
            _data.width = BinaryStreamOperator.ReadSize(reader);
            _data.height = BinaryStreamOperator.ReadSize(reader);
            _data.unknown4 = BinaryStreamOperator.ReadBinary(reader, 32);
            _data.string4.position = BinaryStreamOperator.ReadSize(reader);
            _data.unknown5 = BinaryStreamOperator.ReadBinary(reader, chunkSize - 104); 
        }

        public override void Export(IFile fileSystem,string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);

            string exportFilePath = System.IO.Path.Combine(exportPath, INDEX_FILENAME);
            string fontJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(exportFilePath, fontJson);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string importRoot = GetFolder(rootPath);
            string importFilePath = System.IO.Path.Combine(importRoot, INDEX_FILENAME);
            string content = fileSystem.ReadText(importFilePath);

            _data = JsonConvert.DeserializeObject<Data>(content);
        }


        public override void WriteBinary(BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes(TAG));
            writer.Write(GetChunkContentSize());
            writer.Write(_data.unknown1);
            writer.Write(_data.string1.position);
            writer.Write(_data.string2.position);
            writer.Write(_data.unknown2);
            writer.Write(_data.string3.position);
            writer.Write(_data.unknown3);
            writer.Write(_data.width);
            writer.Write(_data.height);
            writer.Write(_data.unknown4);
            writer.Write(_data.string4.position);
            writer.Write(_data.unknown5);
        }

        public override int GetChunkContentSize()
        {
            return _data.unknown1.Length + 
                   4 +                      // String 1
                   4 +                      // String 2
                   _data.unknown2.Length +
                   4 +                      // String 3
                   _data.unknown3.Length +
                   4 +                      // Window Width
                   4 +                      // Window Height
                   _data.unknown4.Length +
                   4 +                      // String 4
                   _data.unknown5.Length;
        }
    }
}
