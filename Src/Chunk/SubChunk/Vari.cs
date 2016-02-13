using System;
using System.Collections.Generic;
using System.IO;

namespace Heartache.Chunk
{
    class Vari : Chunk
    {
        const string TAG = "VARI";
        const string INDEX_FILENAME = "index.txt";

        List<NamedElement> elementList = new List<NamedElement>();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpSingleNamedFixedSize(reader, 8, elementList);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            ChunkOperator.ExportSingleNamedFixedSize(fileSystem, folderPath, INDEX_FILENAME, elementList);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            ChunkOperator.ImportSingleNamedFixedSize(fileSystem, folderPath, INDEX_FILENAME, elementList);
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            ChunkOperator.WriteSingleNamedFixedSize(writer, TAG, elementList);
        }
    }
}
