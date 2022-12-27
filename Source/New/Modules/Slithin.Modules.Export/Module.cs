using AuroraModularis.Core;
using Slithin.Core.Services;
using Slithin.Modules.Export.Models;

namespace Slithin.Modules.Export;

[Priority(ModulePriority.High)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<IExportProviderFactory>(new ExportProviderFactoryImpl());
        container.Register<IRenderingService>(new RenderingServiceImpl());
    }
}
