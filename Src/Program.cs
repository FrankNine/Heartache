using System;
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
