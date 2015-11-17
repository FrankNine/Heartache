using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Heartache.Chunk
{
    class Strg : Chunk
    {
        class Data
        {
            public List<string> stringList = new List<string>();
        }

        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpString(reader, _data.stringList);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);
            string serializedString = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(System.IO.Path.Combine(exportPath, "Strg.txt"), serializedString);
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
