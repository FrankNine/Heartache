namespace Heartache
{
    public interface IFile
    {
        void CreateDirectoryWithoutReadOnly(string path);
        void WriteText(string fileName, string text);
        void WriteBinary(string fileName, byte[] dataTodump);
    }
}