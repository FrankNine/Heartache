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

    public class ChunkOperator
    {
        public static void DumpSingleNamedArray(BinaryReader reader, ref int chunkSize, List<NamedElement> elementList)
        {
            
        }

        public static void ExportSingleNamedArray(IFile fileSystem,
                                                  string folderPath,
                                                  string indexFilename,
                                                  List<NamedElement> elementList)
        {
            
        }

        public static void ImportSingleNamedArray(IFile fileSystem,
                                                  string folderPath,
                                                  string indexFilename,
                                                  List<NamedElement> elementList)
        {
            
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
