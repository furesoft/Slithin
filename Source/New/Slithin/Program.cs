using AuroraModularis;

public class Program
{
    [STAThread]
    public static Task Main()
    {
        return BootstrapperBuilder.StartConfigure()
            .WithAppName("Slithin")
            .WithModulesBasePath(".")
            .WithSettingsBasePath(".")
            .BuildAndStartAsync();
    }
}
