namespace Heartache.Chunk
{
    class Extn : WholeChunk
    {
        public const string TAG = "EXTN";
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
