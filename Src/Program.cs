using System;
using System.IO;


namespace Heartache
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            BinaryReader reader = FileIO.GetDataWinBinaryReader();
            string outputPath = FileIO.GetOutputPath();

            // Main Chunk FORM
            string mainChunkTag = BinaryStreamOperator.ReadChunkTag(reader);
            if (!string.Equals(mainChunkTag, "FORM"))
            {
                Console.WriteLine("Cannot find main chunk tag at the begining of the file, abort");
                return;
            }

            int formChunkSize = BinaryStreamOperator.ReadSize(reader);
            Console.WriteLine("FORM chunk size: " + formChunkSize);

            int readByte = 0;

            while (readByte < formChunkSize)
            {
                string chunkTag = BinaryStreamOperator.ReadChunkTag(reader, ref readByte);
                switch (chunkTag)
                {
                    case "GEN8":
                    case "OPTN":
                    case "EXTN":
                    case "SOND":
                        _DumpChunkAsAWhole(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "SPRT":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "BGND":
                    case "PATH":
                    case "SCPT":
                        _DumpChunkAsAWhole(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "FONT":
                        _DumpDoubleNamedArray(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "TMLN":
                        _DumpChunkAsAWhole(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "OBJT":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "ROOM":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "DAFL":
                        _DumpChunkAsAWhole(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "TPAG":
                        _DumpChunkAsAWhole(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "CODE":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "VARI":
                        _DumpSingleNamedFixedSize(Path.Combine(outputPath, chunkTag), reader, "VARI_", 8, ref readByte);
                        break;
                    case "FUNC":
                        _DumpSingleNamedFixedSize(Path.Combine(outputPath, chunkTag), reader, "FUNC_", 8, ref readByte);
                        break;
                    case "STRG":
                        _DumpString(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "TXTR":
                        _DumpTexture(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                    case "AUDO":
                        _DumpAudio(Path.Combine(outputPath, chunkTag), reader, ref readByte);
                        break;
                }
            }

            reader.Dispose();
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }




        static void _DumpChunkAsAWhole(string path, BinaryReader reader, ref int readByte)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader, ref readByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);
            FileIO.DumpToFile(Path.Combine(path, "0"), BinaryStreamOperator.ReadBinary(reader, chunkSize, ref readByte));
        }


        static void _DumpString(string path, BinaryReader reader, ref int readByte)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader, ref readByte);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader, ref readByte);

            FileIO.CreateDirectoryWithoutReadOnly(path);
            StreamWriter file = File.CreateText(Path.Combine(path, "STRG.txt"));
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader, ref readByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                file.WriteLine(BinaryStreamOperator.ReadPascalString(reader, ref readByte));
            }

            reader.BaseStream.Seek(chunkStartingPosition + chunkSize, SeekOrigin.Begin);
            file.Close();
        }

        static void _DumpTexture(string path, BinaryReader reader, ref int readByte)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader, ref readByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader, ref readByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader, ref readByte);
            }

            int[] pngPositions = new int[elementCount];
            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);

                BinaryStreamOperator.ReadPosition(reader, ref readByte);
                pngPositions[i] = BinaryStreamOperator.ReadPosition(reader, ref readByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int pngPosition = pngPositions[i];
                reader.BaseStream.Seek(pngPosition, SeekOrigin.Begin);

                long elementDataLength = (((i != elementCount - 1) ? pngPositions[i + 1] : (chunkSize + chunkStartingPosition)) - pngPositions[i]);

                FileStream file = File.Create(Path.Combine(path, i.ToString() + ".png"), (int)elementDataLength);
                file.Write(BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength, ref readByte), 0, (int)elementDataLength);
                file.Close();
            }
        }

        static void _DumpAudio(string path, BinaryReader reader, ref int readByte)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader, ref readByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);
            long chunkStartingPosition = reader.BaseStream.Position;
            int elementCount = BinaryStreamOperator.ReadSize(reader, ref readByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader, ref readByte);
            }
            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                long elementDataLength = BinaryStreamOperator.ReadSize(reader, ref readByte);
                FileStream file = File.Create(Path.Combine(path, i.ToString() + ".wav"), (int)elementDataLength);
                file.Write(BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength, ref readByte), 0, (int)elementDataLength);
                file.Close();
            }
        }

        static void _DumpSingleNamedFixedSize(string path, BinaryReader reader, string prefix, int elementSize, ref int readByte)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader, ref readByte);
            if (chunkSize == 0) { return; }
            int elementCount = chunkSize / (4 + elementSize);

            FileIO.CreateDirectoryWithoutReadOnly(path);

            for (int i = 0; i < elementCount; i++)
            {
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader, ref readByte);
                long elementDataStartPosition = reader.BaseStream.Position;
                reader.BaseStream.Seek(elementNamePosition - 4, SeekOrigin.Begin);
                string sprtName = BinaryStreamOperator.ReadPascalString(reader, ref readByte);

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);
                FileStream file = File.Create(Path.Combine(path, prefix + sprtName), elementSize);
                file.Write(BinaryStreamOperator.ReadBinary(reader, elementSize, ref readByte), 0, elementSize);
                file.Close();
            }
        }

        static void _DumpSingleNamedArray(string path, BinaryReader reader, ref int readByte)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader, ref readByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader, ref readByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader, ref readByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];

                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                int elementNamePosition = BinaryStreamOperator.ReadPosition(reader, ref readByte);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 4;

                reader.BaseStream.Seek(elementNamePosition - 4, SeekOrigin.Begin);
                string sprtName = BinaryStreamOperator.ReadPascalString(reader, ref readByte);

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);

                FileStream file = File.Create(Path.Combine(path, sprtName), (int)elementDataLength);
                file.Write(BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength, ref readByte), 0, (int)elementDataLength);
                file.Close();
            }
        }

        static void _DumpDoubleNamedArray(string path, BinaryReader reader, ref int readByte)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader, ref readByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = BinaryStreamOperator.ReadSize(reader, ref readByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = BinaryStreamOperator.ReadPosition(reader, ref readByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];

                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                int firstElementNamePosition = BinaryStreamOperator.ReadPosition(reader, ref readByte);
                int secondElementNamePosition = BinaryStreamOperator.ReadPosition(reader, ref readByte);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 8;

                reader.BaseStream.Seek(firstElementNamePosition - 4, SeekOrigin.Begin);
                string firstElementName = BinaryStreamOperator.ReadPascalString(reader, ref readByte);

                reader.BaseStream.Seek(secondElementNamePosition - 4, SeekOrigin.Begin);
                string secondElementName = BinaryStreamOperator.ReadPascalString(reader, ref readByte);

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);

                FileStream file = File.Create(Path.Combine(path, firstElementName + ' ' + secondElementName), (int)elementDataLength);
                file.Write(BinaryStreamOperator.ReadBinary(reader, (int)elementDataLength, ref readByte), 0, (int)elementDataLength);
                file.Close();
            }
        }
    }
}
