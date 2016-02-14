using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Heartache.Chunk
{
    abstract class SingleNamedArrayChunk : Chunk
    {
        List<NamedElement> elementList = new List<NamedElement>();

        protected abstract string GetTag();
        protected abstract string GetIndexFilename();

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, GetTag());
        }

        public override int GetChunkContentSize()
        {
            int elementCount = elementList.Count;
            return 4 +                                      // Element Count
                   elementCount * 4 +                       // Element Pointers
                   elementCount * 4 +                       // Element Name String Pointers
                   elementList.Sum(e => e.content.Length);  // Element Content
        }

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            int chunkStartingPosition = (int)reader.BaseStream.Position;

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
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 4;

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);
                elementList.Add(
                    new NamedElement
                    {
                        nameStringPosition = elementNamePosition,
                        content = BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength)
                    }
                );
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(folderPath);
            List<NamedElementFilename> filenameList = new List<NamedElementFilename>();

            for (int i = 0; i < elementList.Count; i++)
            {
                string filename = System.IO.Path.Combine(folderPath, i.ToString());
                fileSystem.WriteBinary(filename, elementList[i].content);
                filenameList.Add(new NamedElementFilename { nameStringPosition = elementList[i].nameStringPosition, filename = filename });
            }

            string indexFullPath = System.IO.Path.Combine(folderPath, GetIndexFilename());
            string indexJson = JsonConvert.SerializeObject(filenameList);
            fileSystem.WriteText(indexFullPath, indexJson);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            string indexFilePath = System.IO.Path.Combine(folderPath, GetIndexFilename());
            string indexJson = fileSystem.ReadText(indexFilePath);
            List<NamedElementFilename> namedElementFilenameList = JsonConvert.DeserializeObject<List<NamedElementFilename>>(indexJson);

            foreach (var namedElementFilename in namedElementFilenameList)
            {
                string filePath = System.IO.Path.Combine(folderPath, namedElementFilename.filename);
                elementList.Add(
                    new NamedElement
                    {
                        nameStringPosition = namedElementFilename.nameStringPosition,
                        content = fileSystem.ReadBinary(filePath)
                    }
                );
            }
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, GetTag());

            int elementCount = elementList.Count;
            writer.Write(GetChunkContentSize());
            writer.Write(elementCount);

            int contentStartingPosition = (int)writer.BaseStream.Position + 4 * elementList.Count;
            int currentContentPosition = contentStartingPosition;

            foreach (var element in elementList)
            {
                writer.Write(currentContentPosition);
                currentContentPosition += (4 + element.content.Length);
            }

            foreach (var element in elementList)
            {
                writer.Write(element.nameStringPosition);
                writer.Write(element.content);
            }
        }
    }
}
