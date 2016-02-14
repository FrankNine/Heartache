namespace Heartache.Chunk
{
    class Dafl : WholeChunk
    {
        public const string TAG = "DAFL";
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