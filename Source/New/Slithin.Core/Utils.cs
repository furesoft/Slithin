using System.Diagnostics;
using System.Runtime.InteropServices;
using AuroraModularis.Core;

namespace Slithin.Core;

public static partial class Utils
{
    public static IEnumerable<T> Find<T>()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic)
            .SelectMany(s => s.GetTypes())
            .Where(x => typeof(T).IsAssignableFrom(x) && x.IsClass)
            .Select(type => (T)Container.Current.Resolve<T>(type));

        return types;
    }

    public static IEnumerable<Type> FindType<T>()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic)
            .SelectMany(s => s.GetTypes())
            .Where(x => typeof(T).IsAssignableFrom(x) && x.IsClass);

        return types;
    }

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
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
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
}
