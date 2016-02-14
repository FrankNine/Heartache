namespace Heartache.Chunk
{
    class Objt : SingleNamedArrayChunk
    {
        const string TAG = "OBJT";
        const string INDEX_FILENAME = "index.txt";

        protected override string GetTag()
        {
            return TAG;
        }

        protected override string GetIndexFilename()
        {
            return INDEX_FILENAME;
        }
    }
}
