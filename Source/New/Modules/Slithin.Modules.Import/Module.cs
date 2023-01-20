using AuroraModularis.Core;
using Slithin.Modules.Import.Models;

namespace Slithin.Modules.Import;

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
        container.Register<IImportProviderFactory>(new ImportProviderFactoryImpl()).AsSingleton();
    }
}
