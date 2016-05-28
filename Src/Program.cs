using System;
using System.Diagnostics;
using System.IO;

namespace Heartache
{
    class Program
    {
        static Chunk.Form form = new Chunk.Form();

        static UI.Heartache heartacheUI = null;

        [STAThread]
        static void Main(string[] args)
        {
            System.Windows.Forms.Application.ApplicationExit += new EventHandler(OnApplicationExit);

            heartacheUI = new UI.Heartache();
            System.Windows.Forms.Application.Run(heartacheUI);
        }

        static void OnApplicationExit(object o, EventArgs e)
        {
            HeartacheSettings.Default.Save();
        }


        public static void GenerateReplaceFontChunks(string glyphGameMakerProjectPath,
                                                     string gameMakerAssetComilerPath,
                                                     string gameMakerBuildTargetPath,
                                                     string replaceFontChunksOutputPath)
        {
            Process process = new Process();

            var gameMakerProcess = new ProcessStartInfo();

            gameMakerProcess.UseShellExecute = true;
            gameMakerProcess.FileName = gameMakerAssetComilerPath;
            gameMakerProcess.WorkingDirectory = Path.GetDirectoryName(gameMakerAssetComilerPath);

            gameMakerProcess.Arguments = string.Format(@"/c /m=win  /config=""Default"" /tgt=64 /obob=True /obpp=False /obru=True /obes=False /i=3 /cvm /tp=2048 /mv=1 /iv=0 /rv=0 /bv=1757 /gn=""UndertaleGlyph"" /td=""C:\Users\franknine\AppData\Local"" /cd=""C:\Users\franknine\Documents\GameMaker\Cache"" /sh=True /dbgp=""6502"" /hip=""127.0.0.1"" /hprt=""51268"" /o=""{0}"" ""{1}""",
                gameMakerBuildTargetPath,
                glyphGameMakerProjectPath);

            Process.Start(gameMakerProcess);
        }

        public static void Disassemble(string dataWinPath,
                                       string disassembledDataPath)
        {
            BinaryReader reader = FileIO.GetDataWinBinaryReader(dataWinPath);

            // Main Chunk FORM
            string mainChunkTag = BinaryStreamOperator.ReadChunkTag(reader);
            if (!string.Equals(mainChunkTag, "FORM"))
            {
                Console.WriteLine("Cannot find main chunk tag at the begining of the file, abort");
                return;
            }

            int formChunkSize = BinaryStreamOperator.ReadSize(reader);
            Console.WriteLine("FORM chunk size: " + formChunkSize);

            var fileIO = new FileIO();

            if (!fileIO.IsDirectoryEmpty(disassembledDataPath))
            {
                fileIO.EmptyDirectory(disassembledDataPath);
            }

            form.ParseBinary(reader);
            form.Export(fileIO, disassembledDataPath);

            reader.Dispose();
        }

        public static void Assemble(string disassembledDataPath,
                                    string translationCSVPath,
                                    string translatedDataWinPath,
                                    string replaceFontPath)
        {
            form.Import(new FileIO(), disassembledDataPath, translationCSVPath, replaceFontPath);
            BinaryWriter writer = FileIO.GetDataWinBinaryWriter(translatedDataWinPath);
            form.WriteBinary(writer);
            writer.Dispose();
        }
    }
}
