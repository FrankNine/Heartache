using System.IO;

namespace Heartache.Actuator
{
    class Assembler
    {
        private Chunk.Form form = new Chunk.Form();

        public void Assemble(string disassembledDataPath,
                             string translationCSVPath,
                             string translatedDataWinPath,
                             string replaceFontPath)
        {
            form.Import(new FileIO(), disassembledDataPath, translationCSVPath, replaceFontPath);
            using (BinaryWriter writer = FileIO.GetDataWinBinaryWriter(translatedDataWinPath))
            {
                form.WriteBinary(writer);
            }
        }
    }
}
