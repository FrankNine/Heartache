using System;
using System.IO;

using Heartache.Chunk;

namespace Heartache
{
    class Program
    {
        static Form form = new Form();

        [STAThread]
        static void Main(string[] args)
        {
            Disassemble();
            Reassemble();
        }

        static void Disassemble()
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

        static void Reassemble()
        {
            form.Import(new FileIO(), FileIO.GetDumpFolderPath());
            BinaryWriter writer = FileIO.GetDataWinBinaryWriter();
            form.WriteBinary(writer);
            writer.Dispose();
        }
    }
}
