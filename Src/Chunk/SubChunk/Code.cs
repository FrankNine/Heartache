namespace Heartache.Chunk
{
    class Code : WholeChunk
    {
        public const string TAG = "CODE";
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
