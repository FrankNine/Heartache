using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heartache.Chunk
{
    class Txtr : Chunk
    {
        List<byte[]> elementList = new List<byte[]>();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpTexture(reader, elementList);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            for (int i = 0; i < elementList.Count; i++)
            {
                fileSystem.WriteText(System.IO.Path.Combine(exportPath, i.ToString()), elementList[i].ToString());
            }
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, "TXTR");
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            throw new NotImplementedException();
        }


        public override void WriteBinary(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
