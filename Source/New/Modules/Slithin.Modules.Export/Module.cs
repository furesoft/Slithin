using AuroraModularis.Core;
using Slithin.Modules.Export.Models;
using Slithin.Modules.Export.Rendering;

namespace Slithin.Modules.Export;

[Priority(ModulePriority.High)]
internal class Module : AuroraModularis.Module
{
    public override Task OnStart(ServiceContainer container)
    {
        var exportProviderFactory = container.Resolve<IExportProviderFactory>();
        exportProviderFactory.Init();

        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        container.Register<IExportProviderFactory>(new ExportProviderFactoryImpl()).AsSingleton();
        container.Register<IRenderingService>(new RenderingServiceImpl()).AsSingleton();
        container.Register<IExportService>(new ExportServiceImpl(container)).AsSingleton();
    }
}
