using Newtonsoft.Json;
using System.Collections.Generic;
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
            List<int> extraFontIndexList = JsonConvert.DeserializeObject<List<int>>(indexJsonContent);

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

        public void Apply()
        {

        }
    }
}
