using System;
using System.Collections.Generic;
using System.IO;

namespace Heartache.Chunk
{
    class Strg : Chunk
    {
        List<string> stringList = new List<string>();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpString(reader, stringList);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            
                fileSystem.WriteText(System.IO.Path.Combine(exportPath, "0"), stringList[0].ToString());
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, "STRG");
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
