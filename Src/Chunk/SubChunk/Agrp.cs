using System;
using System.IO;

namespace Heartache.Chunk
{
    class Agrp : Chunk
    {
        int chunkSize = 0;
        byte[] content = null;

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpChunkAsAWhole(reader, ref chunkSize, ref content);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            fileSystem.WriteBinary(System.IO.Path.Combine(exportPath, "0"), content);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, "AGRP");
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string importPath = GetFolder(rootPath);
            string agrpFilePath = System.IO.Path.Combine(importPath, "0");
            content = fileSystem.ReadBinary(agrpFilePath);
        }


        public override void WriteBinary(BinaryWriter writer)
        {
            writer.Write(content);
        }
    }
}
