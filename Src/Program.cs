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




        public static void Disassemble()
        {
            BinaryReader reader = FileIO.GetDataWinBinaryReader();
            string outputPath = FileIO.GetDumpFolderPath();

            // Main Chunk FORM
            string mainChunkTag = BinaryStreamOperator.ReadChunkTag(reader);
            if (!string.Equals(mainChunkTag, "FORM"))
            {
                Console.WriteLine("Cannot find main chunk tag at the begining of the file, abort");
                return;
            }

            int formChunkSize = BinaryStreamOperator.ReadSize(reader);
            Console.WriteLine("FORM chunk size: " + formChunkSize);

            form.ParseBinary(reader);
            form.Export(new FileIO(), outputPath);

            reader.Dispose();
        }

        public static void Assemble()
        {
            form.Import(new FileIO(), FileIO.GetDumpFolderPath());
            BinaryWriter writer = FileIO.GetDataWinBinaryWriter();
            form.WriteBinary(writer);
            writer.Dispose();
        }
    }
}
