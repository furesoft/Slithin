using System.Runtime.CompilerServices;
using System.Windows.Input;
using AuroraModularis.Logging.Models;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Settings.ViewModels;

internal class SettingsPageViewModel : BaseViewModel
{
    private readonly LoginInfo _credential;
    private readonly ILogger _logger;
    private readonly ILoginService _loginService;
    private readonly IPathManager _pathManager;
    private readonly SettingsModel _settings;
    private readonly ISettingsService _settingsService;
    private readonly IXochitlService _xochitl;

    public SettingsPageViewModel(ILoginService loginService,
                                 ISettingsService settingsService,
                                 IPathManager pathManager,
                                 IXochitlService xochitl,
                                 ILogger logger)
    {
        _credential = loginService.GetCurrentCredential();
        _loginService = loginService;

        FeedbackCommand = new DelegateCommand(Feedback);

        _settingsService = settingsService;
        _pathManager = pathManager;
        _xochitl = xochitl;
        _logger = logger;

        _settings = settingsService.GetSettings();

        _credential = _loginService.GetCurrentCredential();
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

    public string DeviceName
    {
        get { return _credential.Name; }
        set
        {
            new Action<string>((v) =>
            {
                UpdateDeviceName(v);
                OnChange();
            }).Debounce()(value);
        }
    }

    public ICommand FeedbackCommand { get; set; }

    public bool IsBigMenuMode
    {
        get { return _settings.IsBigMenuMode; }
        set { _settings.IsBigMenuMode = value; SaveSetting(); }
    }

    public bool IsDarkMode
    {
        get { return _settings.IsDarkMode; }
        set { _settings.IsDarkMode = value; SaveSetting(); }
    }

    public bool IsSSHLogin
    {
        get { return _loginService.GetCurrentCredential().Key != null; }
    }

    private void Feedback(object obj)
    {
        var feedbackWindow = new FeedbackWindow();
        feedbackWindow.Show();
    }

    private void SaveSetting([CallerMemberName] string property = null)
    {
        _settingsService.Save(_settings);

        _logger.Info($"Setting changed '{property}'");

        OnChange(property);
    }

    private void UpdateDeviceName(string newName)
    {
        string oldName = DeviceName;

        if (string.IsNullOrEmpty(newName)) return;

        _pathManager.ReLink(newName);

        var path = new DirectoryInfo(_pathManager.ConfigBaseDir);

        if (path.Exists)
        {
            return;
        }

        _pathManager.ReLink(oldName);
        path = new(_pathManager.ConfigBaseDir);

        _credential.Name = newName;

        _pathManager.ReLink(_credential.Name);

        path.MoveTo(_pathManager.ConfigBaseDir);

        _loginService.UpdateLoginCredential(_credential);

        _logger.Info("Setting changed 'Device Name'");
    }
}
