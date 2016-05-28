using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Heartache.Chunk
{
    public class Form : Chunk
    {
        const string TAG = "FORM";

        Gen8 gen8 = new Gen8();
        Optn optn = new Optn();
        Extn extn = new Extn();
        Sond sond = new Sond();
        Agrp agrp = new Agrp();
        Sprt sprt = new Sprt();
        Bgnd bgnd = new Bgnd();
        Path path = new Path();
        Scpt scpt = new Scpt();
        Shdr shdr = new Shdr();
        Font font = new Font();
        Tmln tmln = new Tmln();
        Objt objt = new Objt();
        Room room = new Room();
        Dafl dafl = new Dafl();
        Tpag tpag = new Tpag();
        Code code = new Code();
        Vari vari = new Vari();
        Func func = new Func();
        Strg strg = new Strg();
        Txtr txtr = new Txtr();
        Audo audo = new Audo();

        public Font GetFont()
        {
            return font;
        }

        private List<Chunk> _allSubChunk = new List<Chunk>();
        public Form()
        {
            _allSubChunk = new List<Chunk> {
                gen8,
                optn,
                extn,
                sond,
                agrp,
                sprt,
                bgnd,
                path,
                scpt,
                shdr,
                font,
                tmln,
                objt,
                room,
                dafl,
                tpag,
                code,
                vari,
                func,
                strg,
                txtr,
                audo
            };
        }

        public override string GetFolder(string rootPath)
        {
            return string.Empty;
        }

        public override void ParseBinary(BinaryReader reader)
        {
            int readByte = 0;

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                string chunkTag = BinaryStreamOperator.ReadChunkTag(reader, ref readByte);

                switch (chunkTag)
                {
                    case Gen8.TAG: gen8.ParseBinary(reader); break;
                    case Optn.TAG: optn.ParseBinary(reader); break;
                    case Extn.TAG: extn.ParseBinary(reader); break;
                    case Sond.TAG: sond.ParseBinary(reader); break;
                    case Agrp.TAG: agrp.ParseBinary(reader); break;
                    case Sprt.TAG: sprt.ParseBinary(reader); break;
                    case Bgnd.TAG: bgnd.ParseBinary(reader); break;
                    case Path.TAG: path.ParseBinary(reader); break;
                    case Scpt.TAG: scpt.ParseBinary(reader); break;
                    case Shdr.TAG: shdr.ParseBinary(reader); break;
                    case Font.TAG: font.ParseBinary(reader); break;
                    case Tmln.TAG: tmln.ParseBinary(reader); break;
                    case Objt.TAG: objt.ParseBinary(reader); break;
                    case Room.TAG: room.ParseBinary(reader); break;
                    case Dafl.TAG: dafl.ParseBinary(reader); break;
                    case Tpag.TAG: tpag.ParseBinary(reader); break;
                    case Code.TAG: code.ParseBinary(reader); break;
                    case Vari.TAG: vari.ParseBinary(reader); break;
                    case Func.TAG: func.ParseBinary(reader); break;
                    case Strg.TAG: strg.ParseBinary(reader); break;
                    case Txtr.TAG: txtr.ParseBinary(reader); break;
                    case Audo.TAG: audo.ParseBinary(reader); break;

                    default: break;
                }
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            _allSubChunk.ForEach(c => c.Export(fileSystem, rootPath));
        }
        
        public override void Import(IFile fileSystem, string rootPath)
        {
            throw new NotImplementedException();
        }

        ExtraStrg exStrg = new ExtraStrg();
        ExtraFont exFont = new ExtraFont();

        public void Import(IFile fileSystem, 
                           string rootPath,
                           string translationCSVPath,
                           string replaceFontPath)
        {
            _allSubChunk.ForEach(c => c.Import(fileSystem, rootPath));

            exFont.Import(fileSystem, replaceFontPath, Font.INDEX_FILE_NAME);
            exStrg.Import(translationCSVPath);
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, TAG);
            writer.Write(GetChunkContentSize() + exStrg.GetSize() + exFont.GetSize());

            _allSubChunk.TakeWhile(c=>c!=strg).ToList().ForEach(c => c.WriteBinary(writer));
            int strgStartingPosition = (int)writer.BaseStream.Position;
            strg.WriteBinary(writer);
            exStrg.Apply(writer, strgStartingPosition, strg.GetChunkContentSize());
            int fontStartingPosition = 0x1DB03C;
            exFont.Apply(writer, fontStartingPosition, strgStartingPosition, strg.GetChunkContentSize() + exStrg.GetSize());

            txtr.WriteBinary(writer);
            audo.WriteBinary(writer);
        }

        public override int GetChunkContentSize()
        {
            return _allSubChunk.Sum(c => c.GetChunkFullSize());
        }
    }
}
