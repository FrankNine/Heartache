namespace Heartache.Primitive
{
    public class StringEntry
    {
        public int index;
        public int position;
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
