using System.IO;

namespace Heartache.Chunk
{
    abstract class WholeChunk : Chunk
    {
        protected byte[] content;

        protected abstract string GetTag();
        protected abstract string GetExportedFilename();

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, GetTag());
        }

        public override int GetChunkContentSize()
        {
            return content.Length;
        }

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { content = new byte[0]; return; }
            content = BinaryStreamOperator.ReadBinary(reader, chunkSize);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(folderPath);

            string fileFullPath = System.IO.Path.Combine(folderPath, GetExportedFilename());

            fileSystem.WriteBinary(fileFullPath, content);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string folderPath = GetFolder(rootPath);

            string fileFullPath = System.IO.Path.Combine(folderPath, GetExportedFilename());
            content = fileSystem.ReadBinary(fileFullPath);
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, GetTag());
            writer.Write(GetChunkContentSize());
            writer.Write(content);
        }
    }
}
