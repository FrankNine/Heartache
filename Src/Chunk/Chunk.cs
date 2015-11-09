using System.IO;

namespace Heartache.Chunk
{
    abstract class Chunk
    {
        public abstract string GetFolder();

        public abstract void ParseBinary(BinaryReader reader);
        public abstract void Export(IFile fileSystem);
        public abstract void Import(IFile fileSystem);
        public abstract void WriteBinary(BinaryWriter writer);
    }
}
