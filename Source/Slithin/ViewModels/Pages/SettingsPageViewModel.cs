using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;

namespace Slithin.ViewModels.Pages;

public class SettingsPageViewModel : BaseViewModel
{
    private bool _automaticUpdates;

    private string _deviceName;

    public SettingsPageViewModel(ILoginService loginService)
    {
        DeviceName = loginService.GetCurrentCredential().Name;

        OpenExternalCommand = new DelegateCommand(OpenExternal);
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

    public ICommand OpenExternalCommand { get; set; }

    private void OpenExternal(object obj)
    {
        Utils.OpenUrl(obj.ToString());
    }
}
