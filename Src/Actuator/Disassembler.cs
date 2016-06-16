using System;
using System.IO;

namespace Heartache.Actuator
{
    class Disassembler
    {
        private Chunk.Form form = new Chunk.Form();

        public void Disassemble(string dataWinPath,
                                string disassembledDataPath)
        {
            using (BinaryReader reader = FileIO.GetDataWinBinaryReader(dataWinPath))
            {
                // Main Chunk FORM
                string mainChunkTag = BinaryStreamOperator.ReadChunkTag(reader);
                if (!string.Equals(mainChunkTag, Chunk.Form.TAG))
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
            }
        }
    }
}
