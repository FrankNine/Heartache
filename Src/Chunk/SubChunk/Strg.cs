using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Heartache.Primitive;

namespace Heartache.Chunk
{
    public class Strg : Chunk
    {
        public const string TAG = "STRG";
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

            string stringList = string.Join(Environment.NewLine, _data.stringList.Select(e=>e.content).ToArray());
            fileSystem.WriteText(System.IO.Path.Combine(exportPath, INDEX_FILENAME), stringList);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            string indexFullPath = System.IO.Path.Combine(folderPath, INDEX_FILENAME);
            string stringList = fileSystem.ReadText(indexFullPath);

            string[] lines = stringList.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for(int i = 0; i < lines.Length;i++)
            {
                _data.stringList.Add(new StringEntry() { index = i, content = lines[i] });
            }
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, TAG);
            int stringCount = _data.stringList.Count();
            int chunkSize = GetChunkContentSize();

            long chunkSizePosition = writer.BaseStream.Position;

            writer.Write(chunkSize);
            writer.Write(stringCount);

            long pointerStartingPosition = writer.BaseStream.Position;

            int originalStringStartingPosition = (int)writer.BaseStream.Position + 4 * stringCount;
            int originalStringCurrentPosition = originalStringStartingPosition;

            writer.Write(originalStringCurrentPosition);
            for (int i = 1; i < stringCount; i++)
            {
                originalStringCurrentPosition += _data.stringList[i - 1].GetSize();
                writer.Write(originalStringCurrentPosition);
            }

            foreach (var line in _data.stringList)
            {
                WriteString(writer, line.content);
            }

            for(int i = 0; i < 82; i++)
            {
                writer.Write('\0');
            }
        }

        public static void WriteString(BinaryWriter writer, string writtenString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(writtenString);
            writer.Write(bytes.Length);
            writer.Write(bytes);
            writer.Write('\0');
        }

        public override int GetChunkContentSize()
        {
            int stringCount = GetStringCount();
            return 4 +                                      // String Count
                   4 * stringCount +                        // String Pointers
                   _data.stringList.Sum(s => s.GetSize()) + 82;  // String
        }

        public int GetStringCount()
        {
            return _data.stringList.Count;
        }
    }
}
