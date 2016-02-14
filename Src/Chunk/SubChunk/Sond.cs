namespace Heartache.Chunk
{
    class Sond : WholeChunk
    {
        public const string TAG = "SOND";
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
