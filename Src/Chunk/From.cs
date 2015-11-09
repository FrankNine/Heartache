using System;
using System.IO;

namespace Heartache.Chunk
{
    class From : Chunk
    {
        Gen8 gen8;
        Optn optn;
        Extn extn;
        Sond sond;
        Agrp agrp;
        Sprt sprt;
        Bgnd bgnd;
        Path path;
        Scpt scpt;
        Shdr shdr;
        Font font;
        Tmln tmln;
        Objt objt;
        Room room;
        Dafl dafl;
        Tpag tpag;
        Code code;
        Vari vari;
        Func func;
        Strg strg;
        Txtr txtr;
        Audo audo;


        public override string GetFolder()
        {
            return string.Empty;
        }

        public override void ParseBinary(BinaryReader reader)
        {
            int readByte = 0;
            string chunkTag = BinaryStreamOperator.ReadChunkTag(reader, ref readByte);

            switch (chunkTag)
            {
                case "GEN8": gen8.ParseBinary(reader); break;
                case "OPTN": optn.ParseBinary(reader); break;
                case "EXTN": extn.ParseBinary(reader); break;
                case "SOND": sond.ParseBinary(reader); break;
                case "SPRT": sprt.ParseBinary(reader); break;
                case "BGND": bgnd.ParseBinary(reader); break;
                case "PATH": path.ParseBinary(reader); break;
                case "SCPT": scpt.ParseBinary(reader); break;
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
            }
        }

        public override void Export(IFile fileSystem)
        {
            throw new NotImplementedException();
        }

        public override void Import(IFile fileSystem)
        {
            throw new NotImplementedException();
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
