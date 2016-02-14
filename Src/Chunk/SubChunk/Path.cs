namespace Heartache.Chunk
{
    class Path : WholeChunk
    {
        public const string TAG = "PATH";
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
