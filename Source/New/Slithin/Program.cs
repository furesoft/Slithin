using AuroraModularis;
using nUpdate;
using Slithin;

[assembly: nUpdateVersion("1.0.0.0")]

public class Program
{
    [STAThread]
    public static Task Main()
    {
        return BootstrapperBuilder.StartConfigure()
            .WithAppName("Slithin")
            .WithModulesBasePath(".")
            .WithSettingsBasePath(".")
            .WithSettingsProvider<LiteDbSettingsProvider>()
            .BuildAndStartAsync();
    }
}
