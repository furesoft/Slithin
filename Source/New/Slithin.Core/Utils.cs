using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using AuroraModularis.Core;

namespace Slithin.Core;

public static class Utils
{
    public static void OpenUrl(string url)
    {
        try
        {
            Process.Start(url);
        }
        catch
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") {CreateNoWindow = true});
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }
    }

    public static bool CheckIfInternetIsAvailable()
    {
        try { 
            var myPing = new Ping();
            var buffer = new byte[32];
            var pingOptions = new PingOptions();
            var reply = myPing.Send("google.com", 1000, buffer, pingOptions);
            
            return reply.Status == IPStatus.Success;
        }
        catch (Exception) {
            return false;
        }
    }
}
