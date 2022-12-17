using Avalonia;
using nUpdate;
using Slithin;

[assembly: nUpdateVersion("1.0.0.0")]

public class Program
{
    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();

    [STAThread]
    public static void Main()
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(Environment.GetCommandLineArgs());
    }
}
