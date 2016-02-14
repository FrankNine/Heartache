using System.IO;

namespace Heartache.Chunk
{
    public abstract class Chunk
    {
        public abstract string GetFolder(string rootPath);

        public abstract int GetChunkContentSize();
        public int GetChunkFullSize()
        {
            return 4 + // Tag
                   4 + // Chunk Content Size
                   GetChunkContentSize();
        }

        public abstract void ParseBinary(BinaryReader reader);
        public abstract void Export(IFile fileSystem, string rootPath);
        public abstract void Import(IFile fileSystem, string rootPath);
        public abstract void WriteBinary(BinaryWriter writer);
    }
}
