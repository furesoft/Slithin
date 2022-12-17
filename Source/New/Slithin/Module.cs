using AuroraModularis;
using AuroraModularis.Logging.Models;

namespace Slithin.Host;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        container.Resolve<ILogger>().Info("Slithin started");

        return Task.CompletedTask;
    }
}
