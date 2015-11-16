using System;
using System.Collections.Generic;
using System.IO;

namespace Heartache.Chunk
{
    class Font : Chunk
    {
        List<DoubleNamedElement> elementList = new List<DoubleNamedElement>();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpDoubleNamedArray(reader, elementList);
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
            return System.IO.Path.Combine(rootPath, "FONT");
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
