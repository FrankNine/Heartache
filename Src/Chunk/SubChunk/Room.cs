using System.Collections.Generic;
using System.IO;

namespace Heartache.Chunk
{
    class Room : Chunk
    {
        const string TAG = "ROOM";
        const string INDEX_FILENAME = "index.txt";

        int chunkSize;
        List<NamedElement> elementList = new List<NamedElement>();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpSingleNamedArray(reader, ref chunkSize, elementList);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            ChunkOperator.ExportSingleNamedArray(fileSystem, GetFolder(rootPath), INDEX_FILENAME, elementList);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            ChunkOperator.ImportSingleNamedArray(fileSystem, GetFolder(rootPath), INDEX_FILENAME, elementList);
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            ChunkOperator.WriteSingleNamedArray(writer, TAG, elementList);
        }
    }
}
