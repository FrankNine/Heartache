﻿using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using Heartache.Primitive;

namespace Heartache.Chunk
{
    class Strg : Chunk
    {
        const string TAG = "STRG";
        const string INDEX_FILENAME = "Strg.txt";

        class Data
        {
            public List<StringEntry> stringList = new List<StringEntry>();
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
                    index = i,
                    position = elementPosition,
                    content = BinaryStreamOperator.ReadPascalString(reader)
                });
            }

            reader.BaseStream.Seek(chunkStartingPosition + chunkSize, SeekOrigin.Begin);
        }

        public StringEntry LookUpStringEntryByPosition(int position)
        {
            return _data.stringList.Find(se => (se.position + 4) == position);
        }

        private StringEntry LookUpStringEntryByIndex(int index)
        {
            return _data.stringList.Find(se => se.index == index);
        }

        public StringEntry GetAdjustedStringPositionByIndex(int index)
        {
            var match = LookUpStringEntryByIndex(index);
            return new StringEntry
            {
                index = match.index,
                content = match.content,
                position = match.position + 4
            };
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            string serializedString = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(System.IO.Path.Combine(exportPath, INDEX_FILENAME), serializedString);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public void RewriteAllStringPosition(int strgStartingPosition)
        {
            int stringCount = _data.stringList.Count;
            _data.stringList.First().position =
                strgStartingPosition +
                4 + // Tag
                4 + // Chunk Size
                4 + // String Count
                4 * stringCount;

            for (int i = 1; i < stringCount; i++)
            {
                _data.stringList[i].position = _data.stringList[i - 1].position + _data.stringList[i - 1].GetSize();
            }
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            string indexFullPath = System.IO.Path.Combine(folderPath, INDEX_FILENAME);
            string jsonContent = fileSystem.ReadText(indexFullPath);
            _data = JsonConvert.DeserializeObject<Data>(jsonContent);
        }


        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, TAG);
            int stringCount = _data.stringList.Count;
            int chunkSize = GetChunkContentSize();

            writer.Write(chunkSize);
            writer.Write(stringCount);

            int startingPosition = (int)writer.BaseStream.Position + 4 * stringCount;
            int currentPosition = startingPosition;

            writer.Write(currentPosition);
            for (int i = 1; i < stringCount; i++)
            {
                currentPosition += _data.stringList[i - 1].GetSize();
                writer.Write(currentPosition);
            }

            foreach (var strg in _data.stringList)
            {
                writer.Write(strg.content.Length);
                writer.Write(Encoding.ASCII.GetBytes(strg.content));
                writer.Write('\0');
            }

            // Padding?
            for (int i = 0; i < 82; i++)
            {
                writer.Write('\0');
            }
        }

        public override int GetChunkContentSize()
        {
            int stringCount = _data.stringList.Count;
            return 4 +                                      // String Count
                   4 * stringCount +                        // String Pointers
                   _data.stringList.Sum(s => s.GetSize()) + // String
                   82;                                      // End Padding
        }
    }
}
