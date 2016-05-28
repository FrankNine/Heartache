using System;
using System.Windows.Forms;

namespace Heartache.UI
{
    public partial class Heartache : Form
    {
        public string DataWinPath
        {
            get { return textBoxDataWinPath.Text; }
        }

        public string DisassembledDataPath
        {
            get { return textBoxDisassembledDataPath.Text; }
        }

        public string TranslationCSVPath
        {
            get { return textBoxTranslationCSVPath.Text; }
        }

        public string UndertaleGameMakerProjectPath
        {
            get { return textBoxUndertaleGameMakerProjectPath.Text; }
        }

        public string GameMakerAssetCompilerPath
        {
            get { return textBoxGameMakerAssetCompilerPath.Text; }
        }

        public string GameMakerBuildTargetPath
        {
            get { return textBoxGameMakerBuildTargetPath.Text; }
        }

        public string ReplaceFontChunksPath
        {
            get { return textBoxReplaceFontChunksPath.Text; }
        }

        public string TranslatedDataWinPath
        {
            get { return textBoxTranslatedDataWinPath.Text; }
        }

        



        public Heartache()
        {
            InitializeComponent();
        }



        private void buttonDisassemble_Click(object sender, EventArgs e)
        {
            Program.Disassemble(DataWinPath, DisassembledDataPath);
        }

        private void buttonGenerateFontChunks_Click(object sender, EventArgs e)
        {
            Program.GenerateReplaceFontChunks(UndertaleGameMakerProjectPath,
                                              GameMakerAssetCompilerPath,
                                              GameMakerBuildTargetPath,
                                              ReplaceFontChunksPath);
        }

        private void buttonAssemble_Click(object sender, EventArgs e)
        {
            Program.Assemble(DisassembledDataPath,
                             TranslationCSVPath,
                             ReplaceFontChunksPath,
                             TranslatedDataWinPath);
        }



        private void textBoxDataWinPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.DataWinPath = textBoxDataWinPath.Text;
        }

        private void textBoxDisassembledDataPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.DisassembledDataPath = textBoxDisassembledDataPath.Text;
        }

        private void textBoxTranslationCSVPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.TranslationCSVPath = textBoxTranslationCSVPath.Text;
        }

        private void textBoxUndertaleGameMakerProjectPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.UndertaleGlyphGameMakerProjectPath = textBoxUndertaleGameMakerProjectPath.Text;
        }

        private void textBoxGameMakerAssetComplerPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.GameMakerAssetCompilerPath = textBoxGameMakerAssetCompilerPath.Text;
        }

        private void textBoxGameMakerBuildTargetPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.GameMakerBuildTargetPath = textBoxGameMakerBuildTargetPath.Text;
        }

        private void textBoxReplaceFontChunksPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.ReplaceFontChunksPath = textBoxReplaceFontChunksPath.Text;
        }

        private void textBoxTranslatedDataWinPath_TextChanged(object sender, EventArgs e)
        {
            HeartacheSettings.Default.TranslatedDataWinPath = textBoxTranslatedDataWinPath.Text;
        }



        private void buttonOpenDataWin_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            textBoxDataWinPath.Text = fileDialog.FileName;
        }

        private void buttonOpenDisassembledDataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            textBoxDisassembledDataPath.Text = folderBrowserDialog.SelectedPath;
        }
        
        private void buttonOpenTranslationCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            textBoxTranslationCSVPath.Text = fileDialog.FileName;
        }

        private void buttonOpenUndertaleGameMakerProjectPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            textBoxUndertaleGameMakerProjectPath.Text = fileDialog.FileName;
        }

        private void buttonOpenGameMakerAssetCompilerPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            textBoxGameMakerAssetCompilerPath.Text = fileDialog.FileName;
        }

        private void buttonOpenGameMakerBuildTargetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            textBoxGameMakerBuildTargetPath.Text = folderBrowserDialog.SelectedPath;
        }
        
        private void buttonOpenReplaceFontChunksPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            textBoxReplaceFontChunksPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void buttonOpenTranslatedDataWinPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.CheckFileExists = false;
            fileDialog.ShowDialog();
            textBoxTranslatedDataWinPath.Text = fileDialog.FileName;
        }

        
    }
}
