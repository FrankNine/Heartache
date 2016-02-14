using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace Heartache.Chunk
{
    class Func : Chunk
    {
        const string TAG = "FUNC";
        const string INDEX_FILENAME = "index.txt";

        class Element12
        {
            public int stringNamePosition;
            public byte[] unknown; 
        }

        class Element16
        {
            public byte[] unknown1;
            public int stringNamePosition;
            public byte[] unknown2;
        }

        class Data
        {
            public List<Element12> elementList12 = new List<Element12>();
            public List<Element16> elementList16 = new List<Element16>();
        }

        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            int elementCount12 = BinaryStreamOperator.ReadSize(reader);

            for (int i = 0; i < elementCount12; i++)
            {
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                byte[] content = BinaryStreamOperator.ReadBinary(reader, 8);

                _data.elementList12.Add(new Element12
                {
                    stringNamePosition = elementNamePosition,
                    unknown = content
                });
            }

            int elementCount16 = BinaryStreamOperator.ReadSize(reader);

            for (int i = 0; i < elementCount16; i++)
            {
                byte[] header = BinaryStreamOperator.ReadBinary(reader, 4);
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                byte[] content = BinaryStreamOperator.ReadBinary(reader, 8);

                _data.elementList16.Add(new Element16
                {
                    unknown1 = header,
                    stringNamePosition = elementNamePosition,
                    unknown2 = content
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
            int chunkSize = GetChunkContentSize();

            writer.Write(chunkSize);
            writer.Write(_data.elementList12.Count);

            foreach (var element in _data.elementList12)
            {
                writer.Write(element.stringNamePosition);
                writer.Write(element.unknown);
            }

            writer.Write(_data.elementList16.Count);

            foreach (var element in _data.elementList16)
            {
                writer.Write(element.unknown1);
                writer.Write(element.stringNamePosition);
                writer.Write(element.unknown2);
            }
        }

        public override int GetChunkContentSize()
        {
            return 4 +                              // 12 byte Element Count
                   _data.elementList12.Count * 12 + // 12 byte Element Content
                   4 +                              // 16 byte Element Count
                   _data.elementList16.Count * 16;  // 16 byte Element Content
        }
    }
}
