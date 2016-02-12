using System;
using System.IO;

namespace Heartache.Chunk
{
    class Sond : Chunk
    {
        const string TAG = "SOND";
        const string FILENAME = "0";

        int chunkSize = 0;
        byte[] content = null;

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpChunkAsAWhole(reader, ref chunkSize, ref content);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportFolder = GetFolder(rootPath);
            ChunkOperator.ExportChunkAsAWhole(fileSystem, exportFolder, FILENAME, content);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string importFolder = GetFolder(rootPath);
            ChunkOperator.ImportChunkAsAWhole(fileSystem, importFolder, FILENAME, ref content);
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            ChunkOperator.WriteChunkAsAWhole(writer, TAG, content);
        }
    }
}
