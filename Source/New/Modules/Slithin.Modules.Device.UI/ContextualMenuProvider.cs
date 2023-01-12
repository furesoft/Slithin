using Slithin.Core.MVVM;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Device.UI;

public class ContextualMenuProvider : IContextualMenuProvider
{
    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        registrar.RegisterFor(UIContext.Device, new ContextualButton("Reload Device", "Material.Refresh", new DelegateCommand(ReloadDevice)));
    }

    private void ReloadDevice(object obj)
    {
        throw new NotImplementedException();
    }
}
