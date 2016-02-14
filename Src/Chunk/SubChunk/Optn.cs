namespace Heartache.Chunk
{
    class Optn : WholeChunk
    {
        public const string TAG = "OPTN";
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
