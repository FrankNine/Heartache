using System.IO;
using System.Windows.Forms;

namespace Heartache
{
    class FileIO
    {
        const string DEBUG_INPUT_PATH_OVERRIDE = @"C:\Undertale-exp\UNDERTALE\data.win";
        const string DEBUG_OUTPUT_PATH_OVERRIDE = @"C:\Undertale-exp\dump";

        public static string GetDataWinPath()
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

        public static void CreateDirectoryWithoutReadOnly(string path)
        {
            Directory.CreateDirectory(path);
            DirectoryInfo info = new DirectoryInfo(path);
            info.Attributes &= ~FileAttributes.ReadOnly;
        }
    }
}
