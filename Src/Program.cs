using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using Microsoft.VisualBasic.FileIO;

namespace Heartache
{
    class Program
    {
        private static UI.Heartache heartacheUI = null;

        [STAThread]
        static void Main(string[] args)
        {
            heartacheUI = new UI.Heartache();

            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            Application.Run(heartacheUI);
        }

        static void OnApplicationExit(object o, EventArgs e)
        {
            HeartacheSettings.Default.Save();
        }

        private static Chunk.Form form = new Chunk.Form();

        private static Dictionary<string, string> translationTableFontIdToCustomFontNameMapping = 
            new Dictionary<string, string>
        {
            { "0", "fnt_wingdings_0" },
            { "1", "fnt_main_1" },
            { "2", "fnt_maintext_2" },
            { "3", "fnt_small_3" },
            { "4", "fnt_plain_4" },
            { "5", "fnt_plainbig_5" },
            { "6", "fnt_dmg_6" },
            { "7", "fnt_curs_7" },
            { "8", "fnt_comicsans_8" },
            { "9", "fnt_papyrus_9" },
            { "10", "fnt_maintext_2_10" },
        };

        const int csvTranslatedStringCol = 1;
        const int csvFontIdCol = 2;

        const string gmxFontSubFolderName = "fonts";

        public static void InjectGlyphRangeToFontSettings(string glyphGameMakerProjectPath,
                                                          string translationCSVPath)
        {
            var customFontCharacterSets = new Dictionary<string, HashSet<char>>();
            translationTableFontIdToCustomFontNameMapping.ToList().ForEach
                (
                    p => customFontCharacterSets.Add(p.Key, new HashSet<char>())
                );

            ParseCharacterFromTranslationCsv(translationCSVPath, customFontCharacterSets);
            InsertCharactersToGameMakerFontFiles(glyphGameMakerProjectPath, customFontCharacterSets);
        }

        private static void ParseCharacterFromTranslationCsv(string translationCSVPath,
                                                             Dictionary<string, HashSet<char>> customFontCharacterSets)
        {
            using (TextFieldParser parser = new TextFieldParser(translationCSVPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadFields(); // Skip header

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    string translatedString = fields[csvTranslatedStringCol];
                    string translationFont = fields[csvFontIdCol];

                    if (!string.IsNullOrEmpty(translatedString))
                    {
                        foreach (char c in translatedString.ToCharArray())
                        {
                            customFontCharacterSets[translationFont].Add(c);
                        }
                    }
                }
            }
        }

        private static void InsertCharactersToGameMakerFontFiles(string glyphGameMakerProjectPath,
                                                                 Dictionary<string, HashSet<char>> customFontCharacterSets)
        {
            string gameMakerProjectFolderPath = Path.GetDirectoryName(glyphGameMakerProjectPath);
            string fontFolderPath = Path.Combine(gameMakerProjectFolderPath, gmxFontSubFolderName);

            foreach (var characterSet in customFontCharacterSets)
            {
                if(characterSet.Value.Count == 0) { continue; }

                string fontId = characterSet.Key;
                string fontName = translationTableFontIdToCustomFontNameMapping[fontId];
                string fontFileName = string.Format("{0}.font.gmx", fontName);
                string fontFilePath = Path.Combine(fontFolderPath, fontFileName);

                XmlDocument fontXml = new XmlDocument();
                fontXml.Load(fontFilePath);
                XmlNode root = fontXml.DocumentElement;
                XmlNode rangesNode = root.SelectSingleNode("/font/ranges");

                foreach(char c in customFontCharacterSets[fontId])
                {
                    XmlElement rangeElement = fontXml.CreateElement("range0");
                    int charDecValue = (int)c;
                    rangeElement.InnerText = string.Format("{0}, {1}", charDecValue, charDecValue);
                    rangesNode.AppendChild(rangeElement);
                }

                fontXml.Save(fontFilePath);
            }
        }


        const string undertaleGlyphProjectName = "UndertaleGlyph.win";

        public static void GenerateReplaceFontChunks(string glyphGameMakerProjectPath,
                                                     string gameMakerAssetComilerPath,
                                                     string gameMakerBuildTargetPath,
                                                     string replaceFontChunksOutputPath)
        {
            var gameMakerAssetCompilerProcess = new ProcessStartInfo();

            gameMakerAssetCompilerProcess.UseShellExecute = true;
            gameMakerAssetCompilerProcess.FileName = gameMakerAssetComilerPath;
            gameMakerAssetCompilerProcess.WorkingDirectory = Path.GetDirectoryName(gameMakerAssetComilerPath);

            gameMakerAssetCompilerProcess.Arguments = string.Format(@"/c /m=win  /config=""Default"" /tgt=64 /obob=True /obpp=False /obru=True /obes=False /i=3 /cvm /tp=2048 /mv=1 /iv=0 /rv=0 /bv=1757 /gn=""UndertaleGlyph"" /td=""C:\Users\franknine\AppData\Local"" /cd=""C:\Users\franknine\Documents\GameMaker\Cache"" /sh=True /dbgp=""6502"" /hip=""127.0.0.1"" /hprt=""51268"" /o=""{0}"" ""{1}""",
                gameMakerBuildTargetPath,
                glyphGameMakerProjectPath);

            Process process = Process.Start(gameMakerAssetCompilerProcess);
            process.WaitForExit();


            string fontDataWinPath = Path.Combine(gameMakerBuildTargetPath, undertaleGlyphProjectName);
            Chunk.Form fontDataWinForm = new Chunk.Form();
            using (BinaryReader reader = FileIO.GetDataWinBinaryReader(fontDataWinPath))
            {
                fontDataWinForm.ParseBinary(reader);
            }

            Chunk.Font font = fontDataWinForm.GetFont();
            font.Export(new FileIO(), replaceFontChunksOutputPath);
        }

        public static void Disassemble(string dataWinPath,
                                       string disassembledDataPath)
        {
            using (BinaryReader reader = FileIO.GetDataWinBinaryReader(dataWinPath))
            {
                // Main Chunk FORM
                string mainChunkTag = BinaryStreamOperator.ReadChunkTag(reader);
                if (!string.Equals(mainChunkTag, Chunk.Form.TAG))
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
            }
        }

        public static void Assemble(string disassembledDataPath,
                                    string translationCSVPath,
                                    string translatedDataWinPath,
                                    string replaceFontPath)
        {
            form.Import(new FileIO(), disassembledDataPath, translationCSVPath, replaceFontPath);
            using (BinaryWriter writer = FileIO.GetDataWinBinaryWriter(translatedDataWinPath))
            {
                form.WriteBinary(writer);
            }
        }
    }
}
