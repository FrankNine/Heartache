namespace Heartache.Chunk
{
    class Shdr : WholeChunk
    {
        public const string TAG = "SHDR";
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
