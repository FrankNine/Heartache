using System;
using System.IO;
using System.Collections.Generic;

namespace Heartache.Chunk
{
    class Audo : Chunk
    {
        List<byte[]> audioList = new List<byte[]>();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpAudio(reader, audioList);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            for (int i = 0; i < audioList.Count; i++)
            {
                fileSystem.WriteBinary(System.IO.Path.Combine(exportPath, i.ToString()), audioList[i]);
            }
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
