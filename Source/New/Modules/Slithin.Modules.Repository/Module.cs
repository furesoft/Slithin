using AuroraModularis;

namespace Slithin.Modules.Repository;

public class Module : AuroraModularis.Module
{
    public override Task OnStart(TinyIoCContainer container)
    {
        return Task.CompletedTask;
    }
}
