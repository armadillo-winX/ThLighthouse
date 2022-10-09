using System;
using System.Diagnostics;

namespace ThLighthouse
{
    internal class VersionInfo
    {
        private readonly static string _appPath = PathInfo.AppPath;

        public static string? AppName => FileVersionInfo.GetVersionInfo(_appPath).ProductName;

        public static string AppVersion => $"{FileVersionInfo.GetVersionInfo(_appPath).ProductVersion}-{Channel}";

        public static string Channel => "Alpha";

        public static string? Developer => FileVersionInfo.GetVersionInfo(_appPath).CompanyName;

        public static string? OSVersion => Environment.OSVersion.ToString();

        public static string? DotNetViersion => $".NET {Environment.Version}";
    }
}
