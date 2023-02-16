using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.BaseServices.Models;

namespace Slithin.Modules.FirstStart.ViewModels;

internal class DeviceStepViewModel : BaseViewModel
{
    private LoginInfo _selectedLogin = new();

    public DeviceStepViewModel(IDeviceDiscovery deviceDiscovery)
    {
        var ip = deviceDiscovery.Discover();

        if (ip != null)
        {
            SelectedLogin.IP = ip.ToString();
        }
    }

    public LoginInfo SelectedLogin
    {
        get { return _selectedLogin; }
        set { SetValue(ref _selectedLogin, value); }
    }
}
