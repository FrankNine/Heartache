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

        private List<Chunk> _allSubChunk = new List<Chunk>();
        public Form()
        {
            _allSubChunk.Add(gen8);
            _allSubChunk.Add(optn);
            _allSubChunk.Add(extn);
            _allSubChunk.Add(sond);
            _allSubChunk.Add(agrp);
            _allSubChunk.Add(sprt);
            _allSubChunk.Add(bgnd);
            _allSubChunk.Add(path);
            _allSubChunk.Add(scpt);
            _allSubChunk.Add(shdr);
            _allSubChunk.Add(font);
            _allSubChunk.Add(tmln);
            _allSubChunk.Add(objt);
            _allSubChunk.Add(room);
            _allSubChunk.Add(dafl);
            _allSubChunk.Add(tpag);
            _allSubChunk.Add(code);
            _allSubChunk.Add(vari);
            _allSubChunk.Add(func);
            _allSubChunk.Add(strg);
            _allSubChunk.Add(txtr);
            _allSubChunk.Add(audo);
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
                Console.WriteLine(chunkTag);
                switch (chunkTag)
                {
                    case "GEN8": gen8.ParseBinary(reader); break;
                    case "OPTN": optn.ParseBinary(reader); break;
                    case "EXTN": extn.ParseBinary(reader); break;
                    case "SOND": sond.ParseBinary(reader); break;
                    case "AGRP": agrp.ParseBinary(reader); break;
                    case "SPRT": sprt.ParseBinary(reader); break;
                    case "BGND": bgnd.ParseBinary(reader); break;
                    case "PATH": path.ParseBinary(reader); break;
                    case "SCPT": scpt.ParseBinary(reader); break;
                    case "SHDR": shdr.ParseBinary(reader); break;
                    case "FONT": font.ParseBinary(reader); break;
                    case "TMLN": tmln.ParseBinary(reader); break;
                    case "OBJT": objt.ParseBinary(reader); break;
                    case "ROOM": room.ParseBinary(reader); break;
                    case "DAFL": dafl.ParseBinary(reader); break;
                    case "TPAG": tpag.ParseBinary(reader); break;
                    case "CODE": code.ParseBinary(reader); break;
                    case "VARI": vari.ParseBinary(reader); break;
                    case "FUNC": func.ParseBinary(reader); break;
                    case "STRG": strg.ParseBinary(reader); break;
                    case "TXTR": txtr.ParseBinary(reader); break;
                    case "AUDO": audo.ParseBinary(reader); break;

                    default: Console.WriteLine("Unhandled data!"); break;
                }
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            _allSubChunk.ForEach(c => c.Export(fileSystem, rootPath));
        }

        public override void Import(IFile fileSystem, string rootPath)
        {
            _allSubChunk.ForEach(c => c.Import(fileSystem, rootPath));
        }

        const string con = "what is it?";
        public override void WriteBinary(BinaryWriter writer)
        {
            BinaryStreamOperator.WriteTag(writer, TAG);
            writer.Write(GetChunkContentSize());

            _allSubChunk.ForEach(c => c.WriteBinary(writer));
        }

        public override int GetChunkContentSize()
        {
            return _allSubChunk.Sum(c => c.GetChunkFullSize());
        }
    }
}
