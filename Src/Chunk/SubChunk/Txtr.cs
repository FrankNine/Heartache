using System.Linq;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Heartache.Chunk
{
    class Txtr : Chunk
    {
        const string TAG = "TXTR";
        const string INDEX_FILENAME = "index.txt";
        List<byte[]> elementList = new List<byte[]>();

        class Data
        {
            public List<string> exportedImageNameList = new List<string>();
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

                BinaryStreamOperator.ReadPosition(reader);
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


        public override void WriteBinary(BinaryWriter writer)
        {
            writer.Write(TAG);
            int textureCount = elementList.Count;
            int chunkSize = 4 + 4 * textureCount + elementList.Sum(s => s.Length);

            writer.Write(chunkSize);
            writer.Write(textureCount);

            int startingPosition = (int)writer.BaseStream.Position + 4 * textureCount;
            int currentPosition = startingPosition;

            writer.Write(currentPosition);
            for (int i = 1; i < textureCount; i++)
            {
                currentPosition += elementList[i - 1].Length;
                writer.Write(currentPosition);
            }

            foreach (var element in elementList)
            {
                writer.Write(element);
            }
        }
    }
}
