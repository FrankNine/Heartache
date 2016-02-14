namespace Heartache.Chunk
{
    class Tpag : WholeChunk
    {
        public const string TAG = "TPAG";
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
