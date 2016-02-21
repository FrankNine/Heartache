namespace Heartache.Chunk
{
    class Room : WholeChunk
    {
        const string TAG = "ROOM";
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
