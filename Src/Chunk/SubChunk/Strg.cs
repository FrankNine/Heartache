using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Heartache.Chunk
{
    class Strg : Chunk
    {
        class Data
        {
            public List<StringEntry> stringList = new List<StringEntry>();
        }

        class StringEntry
        {
            public long position;
            public string content;
        }

        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader);

            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                _data.stringList.Add(new StringEntry
                {
                    position = elementPosition,
                    content = BinaryStreamOperator.ReadPascalString(reader)
                });
            }

            reader.BaseStream.Seek(chunkStartingPosition + chunkSize, SeekOrigin.Begin);
        }

        public void LookUpStringIndexAndContent(long position, ref int index, ref string content)
        {
            index = _data.stringList.FindIndex(se => se.position+4 == position);
            content = _data.stringList[index].content;
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            string serializedString = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(System.IO.Path.Combine(exportPath, "Strg.txt"), serializedString);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, "STRG");
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
