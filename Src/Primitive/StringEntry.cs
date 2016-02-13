namespace Heartache.Primitive
{
    class StringEntry
    {
        public int index;
        public uint position;
        public string content;

        public int GetSize()
        {
            return 4 + content.Length;
        }

        public static int GetStringPointerSize()
        {
            return 4;
        }
    }
}
