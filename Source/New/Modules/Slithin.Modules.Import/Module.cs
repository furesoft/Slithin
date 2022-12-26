using AuroraModularis.Core;
using Slithin.Core.Services.Implementations;
using Slithin.Modules.Import.Models;

namespace Slithin.Modules.Logging;

[Priority]
public class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        var factoryProvider = container.Resolve<IImportProviderFactory>();
        factoryProvider.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IImportProviderFactory>(new ImportProviderFactoryImpl());
    }
}
