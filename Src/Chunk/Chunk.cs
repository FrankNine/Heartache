using System.Collections.Generic;
using System.IO;

namespace Heartache.Chunk
{
    public abstract class Chunk
    {
        public abstract string GetFolder(string rootPath);

        public abstract void ParseBinary(BinaryReader reader);
        public abstract void Export(IFile fileSystem, string rootPath);
        public abstract void Import(IFile fileSystem, string rootPath);
        public abstract void WriteBinary(BinaryWriter writer);
    }
}
