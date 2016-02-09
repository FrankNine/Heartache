using System.IO;
using System.Windows.Forms;

namespace Heartache
{
    class FileIO : IFile
    {
        const string DEBUG_INPUT_DATA_PATH_OVERRIDE = @"C:\Projects\Underminer\data.win";
        const string DEBUG_DUMP_PATH_OVERRIDE = @"C:\Undertale-exp\dump";
        const string DEBUG_OUTPUT_DATA_PATH_OVERRIDE = @"C:\Projects\Underminer\data-r.win";

        public static BinaryReader GetDataWinBinaryReader()
        {
            string dataWinPath = _GetDataWinInputPath();
            FileStream stream = new FileStream(dataWinPath, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);

            return reader;
        }

        public static BinaryWriter GetDataWinBinaryWriter()
        {
            string dataWinPath = _GetDataWinOutputPath();

            FileStream stream = new FileStream(dataWinPath, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(stream);

            return writer;
        }

        static string _GetDataWinInputPath()
        {
            if (!string.IsNullOrEmpty(DEBUG_INPUT_DATA_PATH_OVERRIDE))
            {
                return DEBUG_INPUT_DATA_PATH_OVERRIDE;
            }

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }

        static string _GetDataWinOutputPath()
        {
            if (!string.IsNullOrEmpty(DEBUG_OUTPUT_DATA_PATH_OVERRIDE))
            {
                return DEBUG_OUTPUT_DATA_PATH_OVERRIDE;
            }

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }

        public static string GetDumpFolderPath()
        {
            if (!string.IsNullOrEmpty(DEBUG_DUMP_PATH_OVERRIDE))
            {
                return DEBUG_DUMP_PATH_OVERRIDE;
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
            if (dataTodump == null) { return; }

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
