using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualBasic.FileIO;

namespace Heartache.Actuator
{
    class GlyphInjector
    {
        private static readonly Dictionary<string, string> translationTableFontIdToCustomFontNameMapping =
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

        public void InjectGlyphRangeToFontSettings(string glyphGameMakerProjectPath,
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
                if (characterSet.Value.Count == 0) { continue; }

                string fontId = characterSet.Key;
                string fontName = translationTableFontIdToCustomFontNameMapping[fontId];
                string fontFileName = string.Format("{0}.font.gmx", fontName);
                string fontFilePath = Path.Combine(fontFolderPath, fontFileName);

                XmlDocument fontXml = new XmlDocument();
                fontXml.Load(fontFilePath);
                XmlNode root = fontXml.DocumentElement;
                XmlNode rangesNode = root.SelectSingleNode("/font/ranges");

                foreach (char c in customFontCharacterSets[fontId])
                {
                    XmlElement rangeElement = fontXml.CreateElement("range0");
                    int charDecValue = (int)c;
                    rangeElement.InnerText = string.Format("{0}, {1}", charDecValue, charDecValue);
                    rangesNode.AppendChild(rangeElement);
                }

                fontXml.Save(fontFilePath);
            }
        }
    }
}