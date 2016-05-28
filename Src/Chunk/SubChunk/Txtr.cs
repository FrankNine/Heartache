using System.Linq;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using System;

namespace Heartache.Chunk
{
    class Txtr : Chunk
    {
        public const string TAG = "TXTR";
        const string INDEX_FILENAME = "index.txt";
        List<byte[]> elementList = new List<byte[]>();

        class Data
        {
            public List<string> exportedImageNameList = new List<string>();
            public List<int> fileInfoUnknown = new List<int>();
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

            int[] pngPositions = new int[elementCount];
            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);

                _data.fileInfoUnknown.Add(BinaryStreamOperator.ReadPosition(reader));
                pngPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int pngPosition = pngPositions[i];
                reader.BaseStream.Seek(pngPosition, SeekOrigin.Begin);

                long elementDataLength = (((i != elementCount - 1) ? pngPositions[i + 1] : (chunkSize + chunkStartingPosition)) - pngPositions[i]);

                byte[] element = BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength);
                elementList.Add(element);
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            for (int i = 0; i < elementList.Count; i++)
            {
                string imageFileName = i.ToString() + ".png";
                fileSystem.WriteBinary(System.IO.Path.Combine(exportPath, imageFileName), elementList[i]);
                _data.exportedImageNameList.Add(imageFileName);
            }

            string indexJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(System.IO.Path.Combine(exportPath, INDEX_FILENAME), indexJson);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            string indexFullPath = System.IO.Path.Combine(folderPath, INDEX_FILENAME);
            string jsonContent = fileSystem.ReadText(indexFullPath);
            _data = JsonConvert.DeserializeObject<Data>(jsonContent);

            foreach(var textureFilename in _data.exportedImageNameList)
            {
                elementList.Add(fileSystem.ReadBinary(System.IO.Path.Combine(folderPath, textureFilename)));
            }
        }

        const int fileInfoPadding = 52;
        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer,TAG);
            int textureCount = elementList.Count;
            int chunkSize = GetChunkContentSize();

            writer.Write(chunkSize);
            writer.Write(textureCount);

            int fileinfoStartingPosition = (int)writer.BaseStream.Position + 4 * textureCount;
            int currentPosition = fileinfoStartingPosition;

            writer.Write(currentPosition);
            for (int i = 1; i < textureCount; i++)
            {
                currentPosition += 8;
                writer.Write(currentPosition);
            }

            int fileStartingPosition = (int)writer.BaseStream.Position + 8 * textureCount + fileInfoPadding;
            currentPosition = fileStartingPosition;

            writer.Write(_data.fileInfoUnknown[0]);
            writer.Write(currentPosition);
            for (int i = 1; i < textureCount; i++)
            {
                writer.Write(_data.fileInfoUnknown[i]);
                currentPosition += elementList[i - 1].Length;
                writer.Write(currentPosition);
            }

            for (int i = 0; i < fileInfoPadding; i++)
            {
                writer.Write('\0');
            }

            foreach (var element in elementList)
            {
                writer.Write(element);
            }


        }

        public override int GetChunkContentSize()
        {
            int textureCount = elementList.Count;
            return 4 +                              // Texture Count
                   4 * textureCount +               // Texture FileInfo Pointers
                   8 * textureCount +               // Texture FileInfo (4 byte Unknown, 4 byte Pointer)
                   elementList.Sum(s => s.Length) + // Texture Content
                   fileInfoPadding;                 // End Padding
        }
    }
}
