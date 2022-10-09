using System.Diagnostics;

namespace ThLighthouse.Handler
{
    internal class ThdatHandler
    {
        public static string Extract(string gameId, string dataPath, string outputPath)
        {
            if (File.Exists($"{outputPath}\\{Path.GetFileName(dataPath)}"))
                File.Delete($"{outputPath}\\{Path.GetFileName(dataPath)}");
            File.Copy(dataPath, $"{outputPath}\\{Path.GetFileName(dataPath)}");

            string thdatPath = $"{Settings.ThtkDirectory}\\thdat.exe";

            if (!File.Exists(thdatPath))
                throw new FileNotFoundException("thdat.exe が見つかりませんでした。");

            Process process = new();
            process.StartInfo.FileName = thdatPath;
            process.StartInfo.Arguments = $"-x {gameId.Replace("Th", "")} {dataPath}";
            process.StartInfo.WorkingDirectory = outputPath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            return process.StandardOutput.ReadToEnd();
        }
    }
}
