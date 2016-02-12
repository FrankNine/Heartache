using System.Linq;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace Heartache.Chunk
{
    class Func : Chunk
    {
        const string TAG = "FUNC";
        const string INDEX_FILENAME = "index.txt";

        List<byte[]> elementList = new List<byte[]>();

        class Data
        {
            public List<string> funcNameList = new List<string>();
        }

        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            ChunkOperator.DumpSingleNamedFixedSize(reader, 8, elementList);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
           
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            
        }

       

        public override void WriteBinary(BinaryWriter writer)
        {
            writer.Write(TAG);
            writer.Write(4 * _data.funcNameList.Count + elementList.Sum(e => e.Length));
            elementList.ForEach(e => writer.Write(e));
        }
    }
}
