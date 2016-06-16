using System.IO;
using System.Diagnostics;

namespace Heartache.Actuator
{
    class ReplaceFontChunkGenerator
    {
        const string undertaleGlyphProjectName = "UndertaleGlyph.win";

        public void GenerateReplaceFontChunks(string glyphGameMakerProjectPath,
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
    }
}
