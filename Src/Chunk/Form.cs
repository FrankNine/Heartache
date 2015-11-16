using System;
using System.IO;

namespace Heartache.Chunk
{
    public class Form : Chunk
    {
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
                }
            }
        }

        public override void Export(IFile fileSystem, string rootPath)
        {
            gen8.Export(fileSystem, rootPath);
            optn.Export(fileSystem, rootPath);
            extn.Export(fileSystem, rootPath);
            sond.Export(fileSystem, rootPath);
            agrp.Export(fileSystem, rootPath);
            sprt.Export(fileSystem, rootPath);
            bgnd.Export(fileSystem, rootPath);
            path.Export(fileSystem, rootPath);
            scpt.Export(fileSystem, rootPath);
            shdr.Export(fileSystem, rootPath);
            font.Export(fileSystem, rootPath);
            tmln.Export(fileSystem, rootPath);
            objt.Export(fileSystem, rootPath);
            room.Export(fileSystem, rootPath);
            dafl.Export(fileSystem, rootPath);
            tpag.Export(fileSystem, rootPath);
            code.Export(fileSystem, rootPath);
            vari.Export(fileSystem, rootPath);
            func.Export(fileSystem, rootPath);
            strg.Export(fileSystem, rootPath);
            txtr.Export(fileSystem, rootPath);
            audo.Export(fileSystem, rootPath);
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
