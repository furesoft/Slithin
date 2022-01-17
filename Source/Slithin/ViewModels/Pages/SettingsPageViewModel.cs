using System.IO;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Models;

namespace Slithin.ViewModels.Pages;

public class SettingsPageViewModel : BaseViewModel
{
    private readonly LoginInfo _credential;
    private readonly ILoginService _loginService;
    private readonly IPathManager _pathManager;
    private readonly Settings _settings;
    private readonly ISettingsService _settingsService;

    public SettingsPageViewModel(ILoginService loginService, ISettingsService settingsService, IPathManager pathManager)
    {
        _credential = loginService.GetCurrentCredential();
        _loginService = loginService;

        OpenExternalCommand = new DelegateCommand(OpenExternal);
        _settingsService = settingsService;
        _pathManager = pathManager;
        _settings = settingsService.GetSettings();
    }

    public bool AutomaticScreenRecovery
    {
        get { return _settings.AutomaticScreenRecovery; }
        set { _settings.AutomaticScreenRecovery = value; SaveSetting(); }
    }

    public bool AutomaticTemplateRecovery
    {
        get { return _settings.AutomaticTemplateRecovery; }
        set { _settings.AutomaticTemplateRecovery = value; SaveSetting(); }
    }

    public bool AutomaticUpdates
    {
        get { return _settings.AutomaticUpdates; }
        set { _settings.AutomaticUpdates = value; SaveSetting(); }
    }

    public string DeviceName
    {
        get { return _credential.Name; }
        set
        {
            _credential.Name = value;
            _loginService.UpdateLoginCredential(_credential);

            var path = new DirectoryInfo(_pathManager.ConfigBaseDir);

            _pathManager.Relink();

            path.MoveTo(_pathManager.ConfigBaseDir);

            OnChange();
        }
    }

    public ICommand OpenExternalCommand { get; set; }

    private void OpenExternal(object obj)
    {
        Utils.OpenUrl(obj.ToString());
    }

    private void SaveSetting()
    {
        _settingsService.Save(_settings);

        OnChange();
    }
}
