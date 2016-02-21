namespace Heartache.Chunk
{
    class Objt : WholeChunk
    {
        const string TAG = "OBJT";
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
