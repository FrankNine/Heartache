using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualBasic.FileIO;

namespace Heartache.Chunk
{
    class ExtraStrg
    {
        Dictionary<int, string> patchDict;

        public void Import(string translationCSVPath)
        {
            patchDict = new Dictionary<int, string>();

            using (TextFieldParser parser = new TextFieldParser(translationCSVPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadFields(); // Skip header

                int i = 0;

                while (!parser.EndOfData)
                {                   
                    string[] fields = parser.ReadFields();
                    string translatedString = fields[1];
                    if (!string.IsNullOrEmpty(translatedString))
                    {
                        patchDict.Add(i, fields[1]);
                    }
                    
                    i++;
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
