using System;
using System.IO;


namespace Heartache
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            string outputPath = FileIO.GetOutputPath();

            BinaryReader reader = FileIO.GetDataWinBinaryReader();

            // Main Chunk FORM
            string chunk = _ReadChunk(reader);

            int formChunkSize = _ReadSize(reader);
            Console.WriteLine("FORM Chunk Size: " + formChunkSize);

            int readedByte = 0;
            while (readedByte < formChunkSize)
            {
                chunk = _ReadChunk(reader, ref readedByte);
                switch (chunk)
                {
                    case "GEN8":
                    case "OPTN":
                    case "EXTN":
                    case "SOND":
                        _DumpVariableSize(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "SPRT":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "BGND":
                    case "PATH":
                    case "SCPT":
                        _DumpVariableSize(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "FONT":
                        _DumpDoubleNamedArray(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "TMLN":
                        _DumpVariableSize(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "OBJT":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "ROOM":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "DAFL":
                        _DumpVariableSize(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "TPAG":
                        _DumpVariableSize(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "CODE":
                        _DumpSingleNamedArray(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "VARI":
                        _DumpSingleNamedFixedSize(Path.Combine(outputPath, chunk), reader, "VARI_", 8, ref readedByte);
                        break;
                    case "FUNC":
                        _DumpSingleNamedFixedSize(Path.Combine(outputPath, chunk), reader, "FUNC_", 8, ref readedByte);
                        break;
                    case "STRG":
                        _DumpString(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "TXTR":
                        _DumpTexture(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                    case "AUDO":
                        _DumpAudio(Path.Combine(outputPath, chunk), reader, ref readedByte);
                        break;
                }
            }


            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        static string _ReadChunk(BinaryReader reader, ref int readedByte)
        {
            return _ReadAsciiCharArrayAsString(reader, 4, ref readedByte);
        }

        static string _ReadChunk(BinaryReader reader)
        {
            return _ReadAsciiCharArrayAsString(reader, 4);
        }

        static string _ReadAsciiPascalString(BinaryReader reader, ref int readedByte)
        {
            int stringLength = _ReadSize(reader, ref readedByte);
            return _ReadAsciiCharArrayAsString(reader, stringLength, ref readedByte);
        }

        static string _ReadAsciiPascalString(BinaryReader reader)
        {
            int stringLength = _ReadSize(reader);
            return _ReadAsciiCharArrayAsString(reader, stringLength);
        }

        static string _ReadAsciiCharArrayAsString(BinaryReader reader, int length, ref int readedByte)
        {
            readedByte += length;
            return _ReadAsciiCharArrayAsString(reader, length);
        }

        static string _ReadAsciiCharArrayAsString(BinaryReader reader, int length)
        {
            char[] chunkArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                chunkArray[i] = reader.ReadChar();
            }
            return new string(chunkArray);
        }

        static int _ReadSize(BinaryReader reader, ref int readedByte)
        {
            readedByte += 4;
            return _ReadSize(reader);
        }

        static int _ReadSize(BinaryReader reader)
        {
            return reader.ReadInt32();
        }

        static int _ReadPosition(BinaryReader reader, ref int readedByte)
        {
            readedByte += 4;
            return reader.ReadInt32();
        }

        static byte[] _ReadBinary(BinaryReader reader, int size, ref int readedByte)
        {
            readedByte += size;
            return reader.ReadBytes(size);
        }

        static void _DumpVariableSize(string path, BinaryReader reader, ref int readedByte)
        {
            int chunkSize = _ReadSize(reader, ref readedByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);

            FileStream file = File.Create(Path.Combine(path, "0"), chunkSize);
            file.Write(_ReadBinary(reader, chunkSize, ref readedByte), 0, chunkSize);
            file.Close();
        }

        static void _DumpString(string path, BinaryReader reader, ref int readedByte)
        {
            int chunkSize = _ReadSize(reader, ref readedByte);
            if (chunkSize == 0) { return; }

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = _ReadSize(reader, ref readedByte);

            FileIO.CreateDirectoryWithoutReadOnly(path);
            StreamWriter file = File.CreateText(Path.Combine(path, "STRG.txt"));
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = _ReadPosition(reader, ref readedByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                file.WriteLine(_ReadAsciiPascalString(reader, ref readedByte));
            }

            reader.BaseStream.Seek(chunkStartingPosition + chunkSize, SeekOrigin.Begin);
            file.Close();
        }

        static void _DumpTexture(string path, BinaryReader reader, ref int readedByte)
        {
            int chunkSize = _ReadSize(reader, ref readedByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = _ReadSize(reader, ref readedByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = _ReadPosition(reader, ref readedByte);
            }

            int[] pngPositions = new int[elementCount];
            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);

                _ReadPosition(reader, ref readedByte);
                pngPositions[i] = _ReadPosition(reader, ref readedByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int pngPosition = pngPositions[i];
                reader.BaseStream.Seek(pngPosition, SeekOrigin.Begin);

                long elementDataLength = (((i != elementCount - 1) ? pngPositions[i + 1] : (chunkSize + chunkStartingPosition)) - pngPositions[i]);

                FileStream file = File.Create(Path.Combine(path, i.ToString() + ".png"), (int)elementDataLength);
                file.Write(_ReadBinary(reader, (int)elementDataLength, ref readedByte), 0, (int)elementDataLength);
                file.Close();
            }
        }

        static void _DumpAudio(string path, BinaryReader reader, ref int readedByte)
        {
            int chunkSize = _ReadSize(reader, ref readedByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);
            long chunkStartingPosition = reader.BaseStream.Position;
            int elementCount = _ReadSize(reader, ref readedByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = _ReadPosition(reader, ref readedByte);
            }
            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];
                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                long elementDataLength = _ReadSize(reader, ref readedByte);
                FileStream file = File.Create(Path.Combine(path, i.ToString() + ".wav"), (int)elementDataLength);
                file.Write(_ReadBinary(reader, (int)elementDataLength, ref readedByte), 0, (int)elementDataLength);
                file.Close();
            }
        }

        static void _DumpSingleNamedFixedSize(string path, BinaryReader reader, string prefix, int elementSize, ref int readedByte)
        {
            int chunkSize = _ReadSize(reader, ref readedByte);
            if (chunkSize == 0) { return; }
            int elementCount = chunkSize / (4 + elementSize);

            FileIO.CreateDirectoryWithoutReadOnly(path);

            for (int i = 0; i < elementCount; i++)
            {
                int elementNamePosition = _ReadPosition(reader, ref readedByte);
                long elementDataStartPosition = reader.BaseStream.Position;
                reader.BaseStream.Seek(elementNamePosition - 4, SeekOrigin.Begin);
                string sprtName = _ReadAsciiPascalString(reader, ref readedByte);

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);
                FileStream file = File.Create(Path.Combine(path, prefix + sprtName), elementSize);
                file.Write(_ReadBinary(reader, elementSize, ref readedByte), 0, elementSize);
                file.Close();
            }
        }

        static void _DumpSingleNamedArray(string path, BinaryReader reader, ref int readedByte)
        {
            int chunkSize = _ReadSize(reader, ref readedByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = _ReadSize(reader, ref readedByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = _ReadPosition(reader, ref readedByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];

                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                int elementNamePosition = _ReadPosition(reader, ref readedByte);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 4;

                reader.BaseStream.Seek(elementNamePosition - 4, SeekOrigin.Begin);
                string sprtName = _ReadAsciiPascalString(reader, ref readedByte);

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);

                FileStream file = File.Create(Path.Combine(path, sprtName), (int)elementDataLength);
                file.Write(_ReadBinary(reader, (int)elementDataLength, ref readedByte), 0, (int)elementDataLength);
                file.Close();
            }
        }

        static void _DumpDoubleNamedArray(string path, BinaryReader reader, ref int readedByte)
        {
            int chunkSize = _ReadSize(reader, ref readedByte);
            if (chunkSize == 0) { return; }

            FileIO.CreateDirectoryWithoutReadOnly(path);

            long chunkStartingPosition = reader.BaseStream.Position;

            int elementCount = _ReadSize(reader, ref readedByte);
            int[] elementPositions = new int[elementCount];

            for (int i = 0; i < elementCount; i++)
            {
                elementPositions[i] = _ReadPosition(reader, ref readedByte);
            }

            for (int i = 0; i < elementCount; i++)
            {
                int elementPosition = elementPositions[i];

                reader.BaseStream.Seek(elementPosition, SeekOrigin.Begin);
                int firstElementNamePosition = _ReadPosition(reader, ref readedByte);
                int secondElementNamePosition = _ReadPosition(reader, ref readedByte);
                long elementDataStartPosition = reader.BaseStream.Position;
                long elementDataLength = (((i != elementCount - 1) ? elementPositions[i + 1] : (chunkSize + chunkStartingPosition)) - elementPositions[i]) - 8;

                reader.BaseStream.Seek(firstElementNamePosition - 4, SeekOrigin.Begin);
                string firstElementName = _ReadAsciiPascalString(reader, ref readedByte);

                reader.BaseStream.Seek(secondElementNamePosition - 4, SeekOrigin.Begin);
                string secondElementName = _ReadAsciiPascalString(reader, ref readedByte);

                reader.BaseStream.Seek(elementDataStartPosition, SeekOrigin.Begin);

                FileStream file = File.Create(Path.Combine(path, firstElementName + ' ' + secondElementName), (int)elementDataLength);
                file.Write(_ReadBinary(reader, (int)elementDataLength, ref readedByte), 0, (int)elementDataLength);
                file.Close();
            }
        }

        
    }
}
