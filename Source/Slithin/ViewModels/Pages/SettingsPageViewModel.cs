using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Serilog;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.Models;

namespace Slithin.ViewModels.Pages;

public class SettingsPageViewModel : BaseViewModel
{
    private readonly LoginInfo _credential;
    private readonly ILogger _logger;
    private readonly ILoginService _loginService;
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;
    private readonly Settings _settings;
    private readonly ISettingsService _settingsService;

    public SettingsPageViewModel(ILoginService loginService,
                                 ISettingsService settingsService,
                                 IPathManager pathManager,
                                 IMailboxService mailboxService,
                                 ILogger logger)
    {
        _credential = loginService.GetCurrentCredential();
        _loginService = loginService;

        CheckForUpdatesCommand = new DelegateCommand(CheckForUpdates);

        _settingsService = settingsService;
        _pathManager = pathManager;
        _mailboxService = mailboxService;
        _logger = logger;

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

    public ICommand CheckForUpdatesCommand { get; set; }

    public string DeviceName
    {
        get { return _credential.Name; }
        set
        {
            UpdateDeviceName(value);

            OnChange();
        }
    }

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

    private void CheckForUpdates(object obj)
    {
        _mailboxService.Post(new Messages.CheckForUpdateMessage());
    }

    private void SaveSetting([CallerMemberName] string property = null)
    {
        _settingsService.Save(_settings);

        _logger.Information($"Setting changed '{property}'");

        OnChange(property);
    }

    private void UpdateDeviceName(string newName)
    {
        _credential.Name = newName;
        _loginService.UpdateLoginCredential(_credential);

        var path = new DirectoryInfo(_pathManager.ConfigBaseDir);

        _pathManager.Relink();

        path.MoveTo(_pathManager.ConfigBaseDir);

        _logger.Information("Setting changed 'Device Name'");
    }
}
