using System.IO;

namespace Heartache.Chunk
{
    class Bgnd : WholeChunk
    {
        public const string TAG = "BGND";
        const string FILENAME = "0";

        protected override string GetTag()
        {
            return TAG;
        }

        protected override string GetExportedFilename()
        {
            return FILENAME;
        }
    }
}
