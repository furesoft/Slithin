using Slithin.Core.MVVM;
using Slithin.Modules.Device.UI.ViewModels;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Device.UI;

internal class ContextualMenuProvider : IContextualMenuProvider
{
    private readonly DevicePageViewModel _viewModel;

    public ContextualMenuProvider(DevicePageViewModel viewModel)
    {
        _viewModel = viewModel;
    }
    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        registrar.RegisterFor(UIContext.Device, new ContextualButton("Reload Device", "Material.Refresh", _viewModel.ReloadDeviceCommand));
    }
}
