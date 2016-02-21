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
            public List<byte[]> paddingList = new List<byte[]>();
        }
        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            int chunkStartingPosition = (int)reader.BaseStream.Position;
            int chunkEndingPosition = chunkStartingPosition + chunkSize;

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

                int elementDataLength = BinaryStreamOperator.ReadSize(reader);
                audioList.Add(BinaryStreamOperator.ReadBinary(reader, elementDataLength));
                int nextElementPosition = (i != elementCount - 1) ? elementPositions[i + 1] : chunkEndingPosition;
                int paddingLength = nextElementPosition - (int)reader.BaseStream.Position;
                _data.paddingList.Add(BinaryStreamOperator.ReadBinary(reader, paddingLength));
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
            int chunkSize = GetChunkContentSize();

            BinaryStreamOperator.WriteTag(writer, TAG);
            writer.Write(chunkSize);
            writer.Write(audioList.Count);

            int audioStartingPosition = (int)writer.BaseStream.Position + 4 * audioList.Count;

            writer.Write(audioStartingPosition);
            for(int i = 1; i < audioList.Count; i++)
            {
                audioStartingPosition += audioList[i - 1].Length + _data.paddingList[i-1].Length + 4;
                writer.Write(audioStartingPosition);
            }

            for (int i = 0; i < audioList.Count; i++)
            {
                writer.Write(audioList[i].Length);
                writer.Write(audioList[i]);
                writer.Write(_data.paddingList[i]);
            }
        }

        public override int GetChunkContentSize()
        {
            return 4 +                                   // Audio Count
                   4 * audioList.Count +                 // Audio Content Pointer
                   4 * audioList.Count +                 // Audio Content Length
                   audioList.Sum(a => a.Length) +        // Audio Content
                   _data.paddingList.Sum(p => p.Length); // Padding
        }
    }
}
