using AuroraModularis.Core;
using Slithin.Modules.Menu.Models.ContextualMenu;

namespace Slithin.Modules.Device.UI;

internal class Module : AuroraModularis.Module
{
    public override Task OnStart(Container container)
    {
        return Task.CompletedTask;
    }
}
