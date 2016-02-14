namespace Heartache.Chunk
{
    class Scpt : WholeChunk
    {
        public const string TAG = "SCPT";
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
