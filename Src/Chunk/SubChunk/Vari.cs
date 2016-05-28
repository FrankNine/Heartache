﻿namespace Heartache.Chunk
{
    class Vari : WholeChunk
    {
        public const string TAG = "VARI";
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
