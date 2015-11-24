using System;
using System.IO;

using Heartache.Primitive;
using Newtonsoft.Json;

namespace Heartache.Chunk
{
    class Gen8 : Chunk
    {
        int chunkSize = 0;
        byte[] content = null;

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

        public void ResolveString(Strg stringChunk)
        {
            _data.string1 = stringChunk.LookUpStringEntryByPosition(_data.string1.position);
            _data.string2 = stringChunk.LookUpStringEntryByPosition(_data.string2.position);
            _data.string3 = stringChunk.LookUpStringEntryByPosition(_data.string3.position);
            _data.string4 = stringChunk.LookUpStringEntryByPosition(_data.string4.position);
        }

        public override void Export(IFile fileSystem,string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);

            string exportFilePath = System.IO.Path.Combine(exportPath, "Index.txt");
            string fontJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(exportFilePath, fontJson);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, "GEN8");
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            throw new NotImplementedException();
        }


        public override void WriteBinary(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
