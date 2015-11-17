using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Heartache.Chunk
{
    class Audo : Chunk
    {
        List<byte[]> audioList = new List<byte[]>();

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
            fileSystem.WriteText(System.IO.Path.Combine(exportPath, "index.txt"), audioIndexJson);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, "AUDO");
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            throw new NotImplementedException();
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
