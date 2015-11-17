using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Heartache.Chunk
{
    class Font : Chunk
    {
        List<DoubleNamedElement> elementList = new List<DoubleNamedElement>();

        class Data
        {
            public class FontElement
            {
                public int firstElementNamePosition;
                public int secondElementNamePosition;
                public byte[] unknown;

                public List<Glyph> glyphList = new List<Glyph>();
            }

            public class Glyph
            {
                public Int16 key;
                public Int16 x;
                public Int16 y;
                public Int16 w;
                public Int16 h;
                public Int16 shift;
                public Int16 offset;
                public Int16 unknown;
            }

            public List<FontElement> fontElementList = new List<FontElement>();
        }
        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
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
                int fontSize = BinaryStreamOperator.ReadSize(reader);
                byte[] unknownSector = BinaryStreamOperator.ReadBinary(reader, 28);
                int glyphCount = BinaryStreamOperator.ReadSize(reader);

                int[] glyphPosition = new int[glyphCount];
                for (int g = 0; g < glyphCount; g++)
                {
                    glyphPosition[g] = BinaryStreamOperator.ReadPosition(reader);
                }

                List<Data.Glyph> glyphList = new List<Data.Glyph>();
                for(int g = 0; g < glyphCount; g++)
                {
                    Data.Glyph glyph = new Data.Glyph();
                    reader.BaseStream.Seek(glyphPosition[g], SeekOrigin.Begin);

                    glyph.key = BinaryStreamOperator.ReadInt16(reader);
                    glyph.x = BinaryStreamOperator.ReadInt16(reader);
                    glyph.y = BinaryStreamOperator.ReadInt16(reader);
                    glyph.w = BinaryStreamOperator.ReadInt16(reader);
                    glyph.h = BinaryStreamOperator.ReadInt16(reader);
                    glyph.shift = BinaryStreamOperator.ReadInt16(reader);
                    glyph.offset = BinaryStreamOperator.ReadInt16(reader);
                    glyph.unknown = BinaryStreamOperator.ReadInt16(reader);

                    glyphList.Add(glyph);
                }

                _data.fontElementList.Add(new Data.FontElement
                {
                    firstElementNamePosition = firstElementNamePosition,
                    secondElementNamePosition = secondElementNamePosition,
                    unknown = unknownSector,
                    glyphList = glyphList
                });
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);

            string exportFilePath = System.IO.Path.Combine(exportPath, "Index.txt");
            string fontJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(exportFilePath, fontJson);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, "FONT");
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            throw new NotImplementedException();
        }

        

        public override void WriteBinary(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
