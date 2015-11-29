using System.IO;
using System.Windows.Forms;

namespace Heartache
{
    class FileIO : IFile
    {
        const string DEBUG_INPUT_PATH_OVERRIDE = @"C:\Undertale-exp\UNDERTALE\data.win";
        const string DEBUG_OUTPUT_PATH_OVERRIDE = @"C:\Undertale-exp\dump";

        public static BinaryReader GetDataWinBinaryReader()
        {
            string dataWinpath = _GetDataWinPath();
            FileStream stream = new FileStream(dataWinpath, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            return reader;
        }

        static string _GetDataWinPath()
        {
            if (!string.IsNullOrEmpty(DEBUG_INPUT_PATH_OVERRIDE))
            {
                return DEBUG_INPUT_PATH_OVERRIDE;
            }

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }

        public static string GetOutputPath()
        {
            if (!string.IsNullOrEmpty(DEBUG_OUTPUT_PATH_OVERRIDE))
            {
                return DEBUG_OUTPUT_PATH_OVERRIDE;
            }

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            return folderBrowserDialog.SelectedPath;
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
            if(dataTodump == null) { return; }

            FileStream file = File.Create(fileName, dataTodump.Length);
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
