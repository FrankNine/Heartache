namespace Heartache.Chunk
{
    class Tmln : WholeChunk
    {
        public const string TAG = "TMLN";
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
