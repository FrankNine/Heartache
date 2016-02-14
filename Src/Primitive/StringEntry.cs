namespace Heartache.Primitive
{
    class StringEntry
    {
        public int index;
        public uint position;
        public string content;

        public int GetSize()
        {
            return 4 +              // String Length
                   content.Length + // String Content
                   1;               // '\0'
        }

        public static int GetStringPointerSize()
        {
            return 4;
        }
    }
}
