using Slithin.Core;
using Slithin.Core.Services;

namespace Slithin.ViewModels.Pages;

public class SettingsPageViewModel : BaseViewModel
{
    private bool _automaticUpdates;

    private string _deviceName;

    public SettingsPageViewModel(ILoginService loginService)
    {
        DeviceName = loginService.GetCurrentCredential().Name;
    }

    public bool AutomaticUpdates
    {
        get { return _automaticUpdates; }
        set { SetValue(ref _automaticUpdates, value); }
    }

    public string DeviceName
    {
        get { return _deviceName; }
        set { _deviceName = value; }
    }
}
