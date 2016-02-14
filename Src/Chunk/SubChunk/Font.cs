using Heartache.Primitive;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Heartache.Chunk
{
    class Font : Chunk
    {
        const string TAG = "FONT";
        const string INDEX_FILE_NAME = "Index.txt";

        class Data
        {
            public List<FontElement> fontElementList = new List<FontElement>();
            public byte[] unknown;

            public int GetSize()
            {
                return 4 + 4 * fontElementList.Count + fontElementList.Sum(f => f.GetSize()) + unknown.Length;
            }
        }

        public class FontElement
        {
            public StringEntry fontName = new StringEntry();
            public StringEntry fontFilename = new StringEntry();
            public int _fontSize;
            public byte[] _unknown;

            public List<Glyph> glyphList = new List<Glyph>();

            public void ReadBinary(BinaryReader reader)
            {
                fontName.position = (uint)BinaryStreamOperator.ReadPosition(reader);
                fontFilename.position = (uint)BinaryStreamOperator.ReadPosition(reader);
                _fontSize = BinaryStreamOperator.ReadSize(reader);
                _unknown = BinaryStreamOperator.ReadBinary(reader, 28);
                int glyphCount = BinaryStreamOperator.ReadSize(reader);

                int[] glyphPosition = new int[glyphCount];
                for (int g = 0; g < glyphCount; g++)
                {
                    glyphPosition[g] = BinaryStreamOperator.ReadPosition(reader);
                }

                for (int g = 0; g < glyphCount; g++)
                {
                    Glyph glyph = new Glyph();
                    reader.BaseStream.Seek(glyphPosition[g], SeekOrigin.Begin);
                    glyph.ReadBinary(reader);
                    glyphList.Add(glyph);
                }
            }

            public int GetSize()
            {
                return StringEntry.GetStringPointerSize() + // fontName
                       StringEntry.GetStringPointerSize() + // fontFileName
                       sizeof(Int32) +                      // fontSize
                       _unknown.Length +                    // Unknown Sector
                       sizeof(Int32) +                      // Glyph Count
                       sizeof(Int32) * glyphList.Count +    // Glyph Pointer
                       Glyph.GetSize() * glyphList.Count;   // Glyph
            }

            public void WriteBinary(BinaryWriter writer)
            {
                writer.Write(fontName.position);
                writer.Write(fontFilename.position);
                writer.Write(_fontSize);
                writer.Write(_unknown);
                writer.Write(glyphList.Count);

                int currentPosition = (int)writer.BaseStream.Position;
                currentPosition += sizeof(Int32) * glyphList.Count;
                glyphList.ForEach(g =>
                {
                    writer.Write(currentPosition);
                    currentPosition += Glyph.GetSize();
                });
                glyphList.ForEach(g => g.WriteBinary(writer));
            }
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

            public void ReadBinary(BinaryReader reader)
            {
                key = BinaryStreamOperator.ReadInt16(reader);
                x = BinaryStreamOperator.ReadInt16(reader);
                y = BinaryStreamOperator.ReadInt16(reader);
                w = BinaryStreamOperator.ReadInt16(reader);
                h = BinaryStreamOperator.ReadInt16(reader);
                shift = BinaryStreamOperator.ReadInt16(reader);
                offset = BinaryStreamOperator.ReadInt16(reader);
                unknown = BinaryStreamOperator.ReadInt16(reader);
            }

            public static int GetSize() { return 8 * sizeof(Int16); }

            public void WriteBinary(BinaryWriter writer)
            {
                writer.Write(key);
                writer.Write(x);
                writer.Write(y);
                writer.Write(w);
                writer.Write(h);
                writer.Write(shift);
                writer.Write(offset);
                writer.Write(unknown);
            }
        }

        Data _data = new Data();

        public override void ParseBinary(BinaryReader reader)
        {
            int chunkSize = BinaryStreamOperator.ReadSize(reader);
            if (chunkSize == 0) { return; }

            int chunkStartingPosition = (int)reader.BaseStream.Position;

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
                var fontElement = new FontElement();
                fontElement.ReadBinary(reader);
                _data.fontElementList.Add(fontElement);
            }

            _data.unknown = BinaryStreamOperator.ReadBinary(reader, chunkStartingPosition + chunkSize - (int)reader.BaseStream.Position);
        }

        public void ResolveString(Strg stringChunk)
        {
            foreach (FontElement font in _data.fontElementList)
            {
                font.fontName = stringChunk.LookUpStringEntryByPosition(font.fontName.position);
                font.fontFilename = stringChunk.LookUpStringEntryByPosition(font.fontFilename.position);
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);

            string exportFilePath = System.IO.Path.Combine(exportPath, INDEX_FILE_NAME);
            string fontJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(exportFilePath, fontJson);
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            string importPath = GetFolder(rootPath);

            string importFilePath = System.IO.Path.Combine(importPath, INDEX_FILE_NAME);
            string fontJson = fileSystem.ReadText(importFilePath);
            _data = JsonConvert.DeserializeObject<Data>(fontJson);
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, TAG);

            writer.Write(_data.GetSize());
            writer.Write(_data.fontElementList.Count);

            int currentPosition = (int)writer.BaseStream.Position;
            currentPosition += sizeof(Int32) * _data.fontElementList.Count;

            _data.fontElementList.ForEach(f =>
            {
                writer.Write(currentPosition);
                currentPosition += f.GetSize();
            });

            _data.fontElementList.ForEach(f => f.WriteBinary(writer));

            writer.Write(_data.unknown);
        }

        public override int GetChunkContentSize()
        {
            return _data.GetSize();
        }
    }
}
