using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Heartache.Chunk
{
    class Vari : Chunk
    {
        const string TAG = "VARI";
        const string INDEX_FILENAME = "index.txt";

        class Data
        {
            public byte[] unknown;
            public List<NamedElement> elementList = new List<NamedElement>();
        }

        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            _data.unknown = BinaryStreamOperator.ReadBinary(reader, 12);
            int elementSize = 16;

            int elementCount = (chunkSize - 12) / (4 + elementSize);

            for (int i = 0; i < elementCount; i++)
            {
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                byte[] content = BinaryStreamOperator.ReadBinary(reader, elementSize);

                _data.elementList.Add(new NamedElement
                {
                    nameStringPosition = elementNamePosition,
                    content = content
                });
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            string indexFullPath = System.IO.Path.Combine(folderPath, INDEX_FILENAME);
            string indexJson = JsonConvert.SerializeObject(_data);
            fileSystem.WriteText(indexFullPath, indexJson);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            string indexFullPath = System.IO.Path.Combine(folderPath, INDEX_FILENAME);
            _data = JsonConvert.DeserializeObject<Data>(fileSystem.ReadText(indexFullPath));
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, TAG);
            int chunkSize = _data.elementList.Count * 20 + _data.unknown.Length;
            writer.Write(chunkSize);
            writer.Write(_data.unknown);
            foreach(var element in _data.elementList)
            {
                writer.Write(element.nameStringPosition);
                writer.Write(element.content);
            }
        }
    }
}
