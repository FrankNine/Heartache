namespace Heartache.Chunk
{
    class Func : WholeChunk
    {
        public const string TAG = "FUNC";
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
