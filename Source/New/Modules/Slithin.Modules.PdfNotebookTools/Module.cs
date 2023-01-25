using AuroraModularis.Core;
using Slithin.Modules.PdfNotebookTools.Validators;

namespace Slithin.Modules.PdfNotebookTools;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(Container container)
    {
        container.Register<CreateNotebookValidator>();
    }
}
