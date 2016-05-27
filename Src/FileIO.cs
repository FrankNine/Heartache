using System.IO;
using System.Linq;

namespace Heartache
{
    class FileIO : IFile
    {
        public static BinaryReader GetDataWinBinaryReader(string dataWinPath)
        {
            FileStream stream = new FileStream(dataWinPath, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            return reader;
        }

        public static BinaryWriter GetDataWinBinaryWriter(string dataWinPath)
        {
            if (File.Exists(dataWinPath))
            {
                File.Delete(dataWinPath);
            }
            FileStream stream = new FileStream(dataWinPath, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(stream);

            return writer;
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public void EmptyDirectory(string path)
        {
            var directory = Directory.CreateDirectory(path);
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public void CreateDirectoryWithoutReadOnly(string path)
        {
            Directory.CreateDirectory(path);
            DirectoryInfo info = new DirectoryInfo(path);
            info.Attributes &= ~FileAttributes.ReadOnly;
        }

        public void WriteText(string fileName, string text)
        {
            File.WriteAllText(fileName, text);
        }

        public void WriteBinary(string fileName, byte[] dataTodump)
        {
            if (dataTodump == null) { return; }

            FileStream file = File.Create(fileName);
            file.Write(dataTodump, offset: 0, count: dataTodump.Length);
            file.Close();
        }

        public string ReadText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        public byte[] ReadBinary(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }
    }
}
