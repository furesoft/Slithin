using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Slithin.Core
{
    public static class UpdateScriptGenerator
    {
        public static void ApplyOtherUpdate(string sourceFolder, string destinationFolder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = GetOtherScript(sourceFolder, destinationFolder) + $" ; {destinationFolder}\\Slithin -updateInstalled", };
            Process.Start(startInfo);
        }

        public static void ApplyUpdate(string sourceFolder, string destinationFolder)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ApplyUpdateWindows(sourceFolder, destinationFolder);
            }
            else
            {
                ApplyOtherUpdate(sourceFolder, destinationFolder);
            }
        }

        public static void ApplyUpdateWindows(string sourceFolder, string destinationFolder)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c " + GetWindowsScript(sourceFolder, destinationFolder) + $" & start \"{destinationFolder}\\Slithin.exe -updateInstalled\"") { CreateNoWindow = true });
        }

        public static string GetOtherScript(string sourceFolder, string destinationFolder)
        {
            return $"mv  -v {sourceFolder}/* {destinationFolder}";
        }

        public static string GetWindowsScript(string sourceFolder, string destinationFolder)
        {
            return $"xcopy {sourceFolder}\\* {destinationFolder}\\* /s /e /i /Y";
        }
    }
}
