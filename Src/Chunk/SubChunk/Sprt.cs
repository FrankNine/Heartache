namespace Heartache.Chunk
{
    class Sprt : WholeChunk
    {
        public const string TAG = "SPRT";
        const string INDEX_FILENAME = "index.txt";

        protected override string GetTag()
        {
            return TAG;
        }

        protected override string GetExportedFilename()
        {
            return INDEX_FILENAME;
        }
    }
}
