using AuroraModularis.Core;
using Slithin.Modules.Templates.UI.Validators;
using Slithin.Modules.Templates.UI.ViewModels;

namespace Slithin.Modules.Templates.UI;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<TemplatesPageViewModel>().AsSingleton();
        container.Register<AddTemplateValidator>().AsSingleton();
    }
}
