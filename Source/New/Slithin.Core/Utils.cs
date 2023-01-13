using System.Diagnostics;
using System.Runtime.InteropServices;
using AuroraModularis.Core;

namespace Slithin.Core;

public static class Utils
{
    public static IEnumerable<T> Find<T>()
    {
        return 
            from type in FindTypes<T>()
            select Container.Current.Resolve<T>(type);
    }

    public static IEnumerable<Type> FindTypes<T>()
    {
        return
            from assembly in AppDomain.CurrentDomain.GetAssemblies()
            where !assembly.IsDynamic
            from type in TryGetTypes(assembly)
            where typeof(T).IsAssignableFrom(type)
            select type;
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

    private static Type[] TryGetTypes(System.Reflection.Assembly s)
    {
        try
        {
            return s.GetTypes();
        }
        catch
        {
            return Array.Empty<Type>();
        }
    }
}
