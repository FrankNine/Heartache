using System;
using System.IO;
using System.Diagnostics;

using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Xml;

namespace Heartache
{
    class Program
    {
        static Chunk.Form form = new Chunk.Form();

        static UI.Heartache heartacheUI = null;

        [STAThread]
        static void Main(string[] args)
        {
            System.Windows.Forms.Application.ApplicationExit += new EventHandler(OnApplicationExit);

            heartacheUI = new UI.Heartache();
            System.Windows.Forms.Application.Run(heartacheUI);
        }

        static void OnApplicationExit(object o, EventArgs e)
        {
            HeartacheSettings.Default.Save();
        }

        static Dictionary<string, string> customFontMapping = new Dictionary<string, string>
        {
            { "2", "fnt_maintext_2" }
        };

        static Dictionary<string, HashSet<char>> customFontCharacterSet = new Dictionary<string, HashSet<char>>
        {
            { "2", new HashSet<char> { } }
        };

        public static void InjectGlyphRangeToFontSettings(string glyphGameMakerProjectPath,
                                                          string translationCSVPath)
        {
            using (TextFieldParser parser = new TextFieldParser(translationCSVPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadFields(); // Skip header

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    string translatedString = fields[1];
                    string translationFont = fields[2];
                    if (!string.IsNullOrEmpty(translatedString))
                    {
                        foreach (char c in translatedString.ToCharArray())
                        {
                            customFontCharacterSet[translationFont].Add(c);
                        }
                    }
                }
            }

            string fontFolderPath = Path.Combine(Path.GetDirectoryName(glyphGameMakerProjectPath), "fonts");
            string fontFilePath = Path.Combine(fontFolderPath, string.Format("{0}.font.gmx", customFontMapping["2"]));

            XmlDocument doc = new XmlDocument();
            doc.Load(fontFilePath);
            XmlNode root = doc.DocumentElement;
            XmlNode rangesNode = root.SelectSingleNode("/font/ranges");

            foreach (char c in customFontCharacterSet["2"])
            {
                XmlElement elem = doc.CreateElement("range0");
                int charDecValue = ((int)c);
                elem.InnerText = string.Format("{0},{1}", charDecValue, charDecValue);
                rangesNode.AppendChild(elem);
             }

            doc.Save(fontFilePath);
        }


        public static void GenerateReplaceFontChunks(string glyphGameMakerProjectPath,
                                                     string gameMakerAssetComilerPath,
                                                     string gameMakerBuildTargetPath,
                                                     string replaceFontChunksOutputPath)
        {
            var gameMakerProcess = new ProcessStartInfo();

            gameMakerProcess.UseShellExecute = true;
            gameMakerProcess.FileName = gameMakerAssetComilerPath;
            gameMakerProcess.WorkingDirectory = Path.GetDirectoryName(gameMakerAssetComilerPath);

            gameMakerProcess.Arguments = string.Format(@"/c /m=win  /config=""Default"" /tgt=64 /obob=True /obpp=False /obru=True /obes=False /i=3 /cvm /tp=2048 /mv=1 /iv=0 /rv=0 /bv=1757 /gn=""UndertaleGlyph"" /td=""C:\Users\franknine\AppData\Local"" /cd=""C:\Users\franknine\Documents\GameMaker\Cache"" /sh=True /dbgp=""6502"" /hip=""127.0.0.1"" /hprt=""51268"" /o=""{0}"" ""{1}""",
                gameMakerBuildTargetPath,
                glyphGameMakerProjectPath);

            Process process = Process.Start(gameMakerProcess);
            process.WaitForExit();


            string fontDataWinPath = Path.Combine(gameMakerBuildTargetPath, "UndertaleGlyph.win");
            BinaryReader reader = FileIO.GetDataWinBinaryReader(fontDataWinPath);
            Chunk.Form fontDataWinForm = new Chunk.Form();
            fontDataWinForm.ParseBinary(reader);

            Chunk.Font font = fontDataWinForm.GetFont();
            font.Export(new FileIO(), replaceFontChunksOutputPath);
        }

        public static void Disassemble(string dataWinPath,
                                       string disassembledDataPath)
        {
            BinaryReader reader = FileIO.GetDataWinBinaryReader(dataWinPath);

            // Main Chunk FORM
            string mainChunkTag = BinaryStreamOperator.ReadChunkTag(reader);
            if (!string.Equals(mainChunkTag, "FORM"))
            {
                Console.WriteLine("Cannot find main chunk tag at the begining of the file, abort");
                return;
            }

            int formChunkSize = BinaryStreamOperator.ReadSize(reader);
            Console.WriteLine("FORM chunk size: " + formChunkSize);

            var fileIO = new FileIO();

            if (!fileIO.IsDirectoryEmpty(disassembledDataPath))
            {
                fileIO.EmptyDirectory(disassembledDataPath);
            }

            form.ParseBinary(reader);
            form.Export(fileIO, disassembledDataPath);

            reader.Dispose();
        }

        public static void Assemble(string disassembledDataPath,
                                    string translationCSVPath,
                                    string translatedDataWinPath,
                                    string replaceFontPath)
        {
            form.Import(new FileIO(), disassembledDataPath, translationCSVPath, replaceFontPath);
            BinaryWriter writer = FileIO.GetDataWinBinaryWriter(translatedDataWinPath);
            form.WriteBinary(writer);
            writer.Dispose();
        }
    }
}
