using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;


using Microsoft.VisualBasic.FileIO;

namespace Heartache.Chunk
{
    class ExtraStrg
    {
        Dictionary<int, string> patchDict;

        public void Import(IFile fileSystem,
                           string originalExportedStrgPath,
                           string translationCSVPath)
        {
            string strgFileContent = fileSystem.ReadText(originalExportedStrgPath);

            string[] originalStrings = strgFileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            List<string> translatedStrings = new List<string>();
            using (TextFieldParser parser = new TextFieldParser(translationCSVPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {                   
                    string[] fields = parser.ReadFields();
                    translatedStrings.Add(fields[1]);
                }
            }
            
            patchDict = new Dictionary<int, string>();
            for(int i = 0; i < originalStrings.Length; i++)
            {
                string originalString = originalStrings[i];
                string translatedString = translatedStrings[i];

                if (!string.Equals(originalString, translatedString))
                {
                    patchDict.Add(i, translatedString);
                }
            }
        }

        public int GetSize()
        {
            return patchDict.Sum(s => (Encoding.UTF8.GetBytes(s.Value).Length + 5));
        }

        public void Apply(BinaryWriter writer, int strgStartingPosition, int strgOriginalSize)
        {
            { 
                long positionCache = writer.BaseStream.Position;
                writer.BaseStream.Position = strgStartingPosition + 4;
                writer.Write(strgOriginalSize + GetSize());
                writer.BaseStream.Position = positionCache;
            }

            int strgPointerStartingPosition = strgStartingPosition + 12;
            foreach (var stringToPatch in patchDict)
            {
                int overwritePosition = strgPointerStartingPosition + stringToPatch.Key * 4;
                int patchStringPosition = (int)writer.BaseStream.Position;

                Strg.WriteString(writer, stringToPatch.Value);

                long positionCache = writer.BaseStream.Position;
                writer.BaseStream.Position = overwritePosition;
                writer.Write(patchStringPosition);
                writer.BaseStream.Position = positionCache;
            }
        }
    }
}
