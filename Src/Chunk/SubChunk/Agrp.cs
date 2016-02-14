namespace Heartache.Chunk
{
    class Agrp : WholeChunk
    {
        public const string TAG = "AGRP";
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
