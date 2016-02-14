using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Text;

namespace Heartache
{
    public class NamedElement
    {
        public int nameStringPosition;
        public byte[] content;
    }

    public class NamedElementFilename
    {
        public int nameStringPosition;
        public string filename;
    }


    public class DoubleNamedElement
    {
        public int firstNameStringPosition;
        public int SecondNameStringPosition;
        public byte[] content;
    }


    public class ChunkOperator
    {
        public static void DumpChunkAsAWhole(BinaryReader reader, ref int chunkSize, ref byte[] content)
        {
            chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { content = new byte[0]; return; }
            content = BinaryStreamOperator.ReadBinary(reader, chunkSize);
        }

        public static void ExportChunkAsAWhole(IFile fileSystem, string folder, string filename, byte[] content)
        {
            fileSystem.CreateDirectoryWithoutReadOnly(folder);
            string fileFullPath = Path.Combine(folder, filename);
            fileSystem.WriteBinary(fileFullPath, content);
        }

        public static void ImportChunkAsAWhole(IFile fileSystem, string folder, string filename, ref byte[] content)
        {
            string fileFullPath = Path.Combine(folder, filename);
            content = fileSystem.ReadBinary(fileFullPath);
        }

        public static void WriteChunkAsAWhole(BinaryWriter writer, string tag, byte[] content)
        {
            writer.Write(Encoding.ASCII.GetBytes(tag));
            writer.Write(content.Length);
            writer.Write(content);
        }

        public static void DumpSingleNamedArray(BinaryReader reader, ref int chunkSize, List<NamedElement> elementList)
        {
            chunkSize = BinaryStreamOperator.ReadSize(reader);
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
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 4;

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);
                elementList.Add(new NamedElement
                {
                    nameStringPosition = elementNamePosition,
                    content = BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength)
                });
            }
        }

        public static void ExportSingleNamedArray(IFile fileSystem,
                                                  string folderPath,
                                                  string indexFilename,
                                                  List<NamedElement> elementList)
        {
            fileSystem.CreateDirectoryWithoutReadOnly(folderPath);
            List<NamedElementFilename> filenameList = new List<NamedElementFilename>();

            for (int i = 0; i < elementList.Count; i++)
            {
                string filename = Path.Combine(folderPath, i.ToString());
                fileSystem.WriteBinary(filename, elementList[i].content);
                filenameList.Add(new NamedElementFilename { nameStringPosition = elementList[i].nameStringPosition, filename = filename });
            }

            string indexFullPath = Path.Combine(folderPath, indexFilename);
            string indexJson = JsonConvert.SerializeObject(filenameList);
            fileSystem.WriteText(indexFullPath, indexJson);
        }

        public static void ImportSingleNamedArray(IFile fileSystem,
                                                  string folderPath,
                                                  string indexFilename,
                                                  List<NamedElement> elementList)
        {
            string indexFilePath = Path.Combine(folderPath, indexFilename);
            string indexJson = fileSystem.ReadText(indexFilePath);
            List<NamedElementFilename> namedElementFilenameList = JsonConvert.DeserializeObject<List<NamedElementFilename>>(indexJson);

            foreach (var namedElementFilename in namedElementFilenameList)
            {
                string filePath = Path.Combine(folderPath, namedElementFilename.filename);
                elementList.Add(new NamedElement
                {
                    nameStringPosition = namedElementFilename.nameStringPosition
                                                  ,
                    content = fileSystem.ReadBinary(filePath)
                });
            }
        }

        public static void WriteSingleNamedArray(BinaryWriter writer, string tag, List<NamedElement> elementList)
        {
            writer.Write(Encoding.ASCII.GetBytes(tag));

            int elementCount = elementList.Count;
            int chunkSize = 4 + elementCount * 4 + elementCount * 4 + elementList.Sum(e => e.content.Length);
            writer.Write(chunkSize);
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


        public static void DumpSingleNamedFixedSize(BinaryReader reader, 
                                                    int elementSize, 
                                                    List<NamedElement> elementList)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }
            int elementCount = chunkSize / (4 + elementSize);

            for (int i = 0; i < elementCount; i++)
            {
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                byte[] content = BinaryStreamOperator.ReadBinary(reader, elementSize);

                elementList.Add(new NamedElement { nameStringPosition = elementNamePosition,
                                                   content = content});
            }
        }

        public static void ExportSingleNamedFixedSize(IFile fileSystem,
                                                      string folderPath,
                                                      string indexFilename,
                                                      List<NamedElement> elementList)
        {
            ExportSingleNamedArray(fileSystem, folderPath, indexFilename, elementList);
        }

        public static void ImportSingleNamedFixedSize(IFile fileSystem,
                                                      string folderPath,
                                                      string indexFilename,
                                                      List<NamedElement> elementList)
        {
            ImportSingleNamedArray(fileSystem, folderPath, indexFilename, elementList);
        }

        public static void WriteSingleNamedFixedSize(BinaryWriter writer, 
                                                     string tag, 
                                                     List<NamedElement> elementList)
        {
            BinaryStreamOperator.WriteTag(writer, tag);
            writer.Write(elementList.Count);

            int fixedSize = 4 + elementList[0].content.Length;

        }
    }
}
