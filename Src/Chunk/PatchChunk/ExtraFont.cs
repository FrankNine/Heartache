using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Heartache.Chunk
{
    class ExtraFont
    {
        Dictionary<int, Font.FontElement> patchDict;

        public void Import(IFile fileSystem,
                           string extraFontFolderPath,
                           string indexFilename)
        {
            string indexPath = System.IO.Path.Combine(extraFontFolderPath, indexFilename);
            string indexJsonContent = fileSystem.ReadText(indexPath);
            //List<int> extraFontIndexList = JsonConvert.DeserializeObject<List<int>>(indexJsonContent);
            List<int> extraFontIndexList = new List<int> { 2 };
            patchDict = new Dictionary<int, Font.FontElement>();
            foreach (int exfontIndex in extraFontIndexList)
            {
                string exfontFilePath = System.IO.Path.Combine(extraFontFolderPath, exfontIndex.ToString());
                string exfontContent = fileSystem.ReadText(exfontFilePath);
                Font.FontElement fontElement = JsonConvert.DeserializeObject<Font.FontElement>(exfontContent);
                patchDict.Add(exfontIndex, fontElement);
            }
        }

        public int GetSize()
        {
            return patchDict.Sum(p=>p.Value.GetSizeWithSpriteFont());
        }

        public void Apply(BinaryWriter writer, 
                          int fontStartingPosition, 
                          int strgStartingPosition, 
                          int patchedStrgSize)
        {
            {
                long positionCache = writer.BaseStream.Position;
                writer.BaseStream.Position = strgStartingPosition + 4;
                writer.Write(patchedStrgSize + GetSize());
                writer.BaseStream.Position = positionCache;
            }

            int fontPointerStartingPosition = fontStartingPosition + 12;
            foreach (var fontToPatch in patchDict)
            {
                int overwritePosition = fontPointerStartingPosition + fontToPatch.Key * 4;
                int patchFontPosition = (int)writer.BaseStream.Position;
                int spriteFontPointerPosition = patchFontPosition + 28;

                fontToPatch.Value.WriteBinary(writer);
                fontToPatch.Value.fontSprite.WriteFontSprite(writer, spriteFontPointerPosition);

                long positionCache = writer.BaseStream.Position;
                writer.BaseStream.Position = overwritePosition;
                writer.Write(patchFontPosition);
                writer.BaseStream.Position = positionCache;
            }
        }
    }
}
