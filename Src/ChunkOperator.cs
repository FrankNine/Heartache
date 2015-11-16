using System.Collections.Generic;
using System.IO;

namespace Heartache
{
    public class NamedElement
    {
        public int nameStringPosition;
        public byte[] content;
    }


    public class DoubleNamedElement
    {
        public int firstNameStringPosition;
        public int SecondNameStringPosition;
        public byte[] content;
    }


    public class ChunkOperator
    {
        public static void DumpChunkAsAWhole(BinaryReader reader, ref int chunkSize, ref byte[] content)
        {
            chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }
            content = BinaryStreamOperator.ReadBinary(reader, chunkSize);
        }

        public static void DumpSingleNamedArray(BinaryReader reader, ref int chunkSize, List<NamedElement> elementList)
        {
            chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];

                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 4;

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);
                elementList.Add(new NamedElement
                {
                    nameStringPosition = elementNamePosition,
                    content = BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength)
                });
            }
        }

        public static void DumpDoubleNamedArray(BinaryReader reader, List<DoubleNamedElement> elementList)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];

                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                int firstElementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                int secondElementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 8;

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);
                byte[] elementData = BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength);

                elementList.Add(new DoubleNamedElement
                {
                    firstNameStringPosition = firstElementNamePosition,
                    SecondNameStringPosition = secondElementNamePosition,
                    content = elementData
                });
            }
        }

        public static void DumpString(BinaryReader reader, List<string> stringList)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader);

            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                stringList.Add(BinaryStreamOperator.ReadPascalString(reader));
            }

            reader.BaseStream.Seek(chunkStartingPosition + chunkSize, SeekOrigin.Begin);
        }

        public static void DumpTexture(BinaryReader reader, List<byte[]> elementList)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            int[] pngPositions = new int[elementCount];
            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);

                BinaryStreamOperator.ReadPosition(reader);
                pngPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int pngPosition = pngPositions[i];
                reader.BaseStream.Seek(pngPosition, SeekOrigin.Begin);

                long elementDataLength = (((i != elementCount - 1) ? pngPositions[i + 1] : (chunkSize + chunkStartingPosition)) - pngPositions[i]);

                byte[] element = BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength);
                elementList.Add(element);
            }
        }

        public static void DumpAudio( BinaryReader reader, List<byte[]> audioList)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;
            int elementCount = BinaryStreamOperator.ReadSize(reader);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                long elementDataLength = BinaryStreamOperator.ReadSize(reader);
                audioList.Add(BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength));
            }
        }

       public static void DumpSingleNamedFixedSize( BinaryReader reader, int elementSize, List<byte[]> elementList)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }
            int elementCount = chunkSize / (4 + elementSize);

            for (int i = 0; i < elementCount; i++)
            {
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader);
                long elementDataStartPosition = reader.BaseStream.Position;

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);
                byte[] content = BinaryStreamOperator.ReadBinary(reader, elementSize);

                elementList.Add(content);
            }
        }
    }
}
