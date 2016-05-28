using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using Heartache.Primitive;

namespace Heartache.Chunk
{
    public class Font : Chunk
    {
        public const string TAG = "FONT";
        public const string INDEX_FILE_NAME = "Index.txt";

        class Data
        {
            public List<int> fontIndexList = new List<int>();
            public byte[] unknown;

            public int GetSize(List<FontElement> fontElementList)
            {
                return 4 + 4 * fontIndexList.Count + fontElementList.Sum(f => f.GetSize()) + unknown.Length;
            }
        }
        Data _data = new Data();

        List<FontElement> _fontElementList = new List<FontElement>();

        public class FontElement
        {
            public StringEntry fontName = new StringEntry();
            public StringEntry fontFilename = new StringEntry();
            public int fontSize;
            public byte[] unknown1;
            public int fontSpritePosition;
            public FontSprite fontSprite = new FontSprite();
            public byte[] unknown2;

            public List<Glyph> glyphList = new List<Glyph>();

            public void ReadBinary(BinaryReader reader)
            {
                fontName.position = BinaryStreamOperator.ReadPosition(reader);
                fontFilename.position = BinaryStreamOperator.ReadPosition(reader);
                fontSize = BinaryStreamOperator.ReadSize(reader);
                unknown1 = BinaryStreamOperator.ReadBinary(reader, 16);

                fontSpritePosition = BinaryStreamOperator.ReadPosition(reader);
                long currentPosition = reader.BaseStream.Position;

                reader.BaseStream.Position = fontSpritePosition;
                fontSprite.x = BinaryStreamOperator.ReadInt16(reader);
                fontSprite.y = BinaryStreamOperator.ReadInt16(reader);
                fontSprite.w = BinaryStreamOperator.ReadInt16(reader);
                fontSprite.h = BinaryStreamOperator.ReadInt16(reader);
                reader.BaseStream.Position += 12;
                fontSprite.txtrIndex = BinaryStreamOperator.ReadInt16(reader);
                reader.BaseStream.Position = currentPosition;

                unknown2 = BinaryStreamOperator.ReadBinary(reader, 8);

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
                       unknown1.Length +                    // Unknown Sector
                       sizeof(Int32) +                      // SpriteFont Pointer
                       unknown2.Length +                    // Unknown Sector
                       sizeof(Int32) +                      // Glyph Count
                       sizeof(Int32) * glyphList.Count +    // Glyph Pointer
                       Glyph.GetSize() * glyphList.Count;   // Glyph
            }

            public int GetSizeWithSpriteFont()
            {
                return GetSize() + FontSprite.GetSize();
            }

            private long fontSpritePositionPosition;
            public void WriteBinary(BinaryWriter writer)
            {
                writer.Write(fontName.position);
                writer.Write(fontFilename.position);
                writer.Write(fontSize);
                writer.Write(unknown1);
                fontSpritePositionPosition = writer.BaseStream.Position;
                writer.Write(fontSpritePosition);
                writer.Write(unknown2);

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

        public class FontSprite
        {
            public Int16 x;
            public Int16 y;
            public Int16 w;
            public Int16 h;

            public Int16 txtrIndex;

            public static int GetSize()
            {
                return 22;
            }

            public void WriteFontSprite(BinaryWriter writer, int pointerPosition)
            {
                int startPosition = (int)writer.BaseStream.Position;

                writer.Write(x);
                writer.Write(y);
                writer.Write(w);
                writer.Write(h);
                writer.Write(0);
                writer.Write(w);
                writer.Write(h);
                writer.Write(w);
                writer.Write(h);
                writer.Write(txtrIndex);

                long currentPosition = writer.BaseStream.Position;
                writer.BaseStream.Position = pointerPosition;
                writer.Write(startPosition);
                writer.BaseStream.Position = currentPosition;
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

                _data.fontIndexList.Add(i);
                _fontElementList.Add(fontElement);
            }

            _data.unknown = BinaryStreamOperator.ReadBinary(reader, chunkStartingPosition + chunkSize - (int)reader.BaseStream.Position);
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            string exportPath = GetFolder(rootPath);
            fileSystem.CreateDirectoryWithoutReadOnly(exportPath);

            string exportFilePath = System.IO.Path.Combine(exportPath, INDEX_FILE_NAME);
            string fontJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            fileSystem.WriteText(exportFilePath, fontJson);

            for (int i = 0; i < _fontElementList.Count; i++)
            {
                string exportElementPath = System.IO.Path.Combine(exportPath, i.ToString());
                string fontElementJson = JsonConvert.SerializeObject(_fontElementList[i], Formatting.Indented);
                fileSystem.WriteText(exportElementPath, fontElementJson);
            }
        }

        public override string GetFolder(string rootPath)
        {
            return System.IO.Path.Combine(rootPath, TAG);
        }
        
        public override void Import(IFile fileSystem, string rootPath)
        {
            string importPath = GetFolder(rootPath);

            string importFilePath = System.IO.Path.Combine(importPath, INDEX_FILE_NAME);
            string fontIndexListJson = fileSystem.ReadText(importFilePath);
            _data = JsonConvert.DeserializeObject<Data>(fontIndexListJson);

            foreach(int fontIndex in _data.fontIndexList)
            {
                string elementPath = System.IO.Path.Combine(importPath, fontIndex.ToString());
                string fontElementJson = fileSystem.ReadText(elementPath);
                FontElement fontElement = JsonConvert.DeserializeObject<FontElement>(fontElementJson);
                _fontElementList.Add(fontElement);
            }
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, TAG);

            writer.Write(_data.GetSize(_fontElementList));
            writer.Write(_data.fontIndexList.Count);

            int currentPosition = (int)writer.BaseStream.Position;
            currentPosition += sizeof(Int32) * _data.fontIndexList.Count;

            _fontElementList.ForEach(f =>
            {
                writer.Write(currentPosition);
                currentPosition += f.GetSize();
            });

            _fontElementList.ForEach(f => f.WriteBinary(writer));
            writer.Write(_data.unknown);
        }

        public override int GetChunkContentSize()
        {
            return _data.GetSize(_fontElementList);
        }
    }
}
