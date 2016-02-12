using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Heartache.Chunk
{
    class Audo : Chunk
    {
        List<byte[]> audioList = new List<byte[]>();
        const string TAG = "AUDO";
        const string INDEX_FILENAME = "index.txt";

        class Data
        {
            public List<string> audioFileNameList = new List<string>();
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
                long elementDataLength = BinaryStreamOperator.ReadSize(reader);
                audioList.Add(BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength));
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            for (int i = 0; i < audioList.Count; i++)
            {
                string audioFileName = i.ToString() + ".wav";
                fileSystem.WriteBinary(System.IO.Path.Combine(exportPath, audioFileName), audioList[i]);
                _data.audioFileNameList.Add(audioFileName);
            }

            string audioIndexJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(System.IO.Path.Combine(exportPath, INDEX_FILENAME), audioIndexJson);
        }
    
        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string importPath = GetFolder(rootPath);
            string indexFilePath = System.IO.Path.Combine(importPath, INDEX_FILENAME);
            string indexString = fileSystem.ReadText(indexFilePath);
            _data = JsonConvert.DeserializeObject<Data>(indexString);

            foreach(var filename in _data.audioFileNameList)
            {
                string audioPath = System.IO.Path.Combine(importPath, filename);
                audioList.Add(fileSystem.ReadBinary(audioPath));
            }
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            int chunkSize = 4 + 4 + 4 * audioList.Count + audioList.Sum(a => a.Length);

            writer.Write(TAG);
            writer.Write(chunkSize);

            int audioStartingPosition = (int)writer.BaseStream.Position + 4 * audioList.Count;

            writer.Write(audioStartingPosition);
            for(int i = 1; i < audioList.Count; i++)
            {
                audioStartingPosition += audioList[i - 1].Length;
                writer.Write(audioStartingPosition);
            }

            foreach(var audio in audioList)
            {
                writer.Write(audio);
            }
        }
    }
}
