﻿using System;
using System.IO;
using System.Text;

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

            _ResolveStringIndex();
        }

        void _ResolveStringIndex()
        {
            gen8.ResolveString(strg);
            font.ResolveString(strg);
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
            gen8.Import(fileSystem, rootPath);
            optn.Import(fileSystem, rootPath);
            extn.Import(fileSystem, rootPath);
            sond.Import(fileSystem, rootPath);
            agrp.Import(fileSystem, rootPath);
            sprt.Import(fileSystem, rootPath);
            bgnd.Import(fileSystem, rootPath);
            path.Import(fileSystem, rootPath);
            scpt.Import(fileSystem, rootPath);
            shdr.Import(fileSystem, rootPath);
            font.Import(fileSystem, rootPath);
            tmln.Import(fileSystem, rootPath);
            objt.Import(fileSystem, rootPath);
            room.Import(fileSystem, rootPath);
            dafl.Import(fileSystem, rootPath);
            tpag.Import(fileSystem, rootPath);
            code.Import(fileSystem, rootPath);
            vari.Import(fileSystem, rootPath);
            func.Import(fileSystem, rootPath);
            strg.Import(fileSystem, rootPath);
            txtr.Import(fileSystem, rootPath);
            audo.Import(fileSystem, rootPath);
        }

        public override void WriteBinary(BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes("FORM"));
            writer.Write(57056094);

            gen8.WriteBinary(writer);
            optn.WriteBinary(writer);
            extn.WriteBinary(writer);
            sond.WriteBinary(writer);
            agrp.WriteBinary(writer);
            sprt.WriteBinary(writer);
            bgnd.WriteBinary(writer);
            path.WriteBinary(writer);
            scpt.WriteBinary(writer);
            shdr.WriteBinary(writer);
            font.WriteBinary(writer);
            tmln.WriteBinary(writer);
            objt.WriteBinary(writer);
            room.WriteBinary(writer);
            dafl.WriteBinary(writer);
            tpag.WriteBinary(writer);
            code.WriteBinary(writer);
            vari.WriteBinary(writer);
            func.WriteBinary(writer);
            strg.WriteBinary(writer);
            txtr.WriteBinary(writer);
            audo.WriteBinary(writer);
        }
    }
}
