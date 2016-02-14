namespace Heartache.Chunk
{
    class Room : SingleNamedArrayChunk
    {
        const string TAG = "ROOM";
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
