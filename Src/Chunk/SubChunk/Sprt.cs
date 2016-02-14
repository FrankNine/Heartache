namespace Heartache.Chunk
{
    class Sprt : SingleNamedArrayChunk
    {
        const string TAG = "SPRT";
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
