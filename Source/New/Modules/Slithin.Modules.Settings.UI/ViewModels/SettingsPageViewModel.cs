using System.Runtime.CompilerServices;
using System.Windows.Input;
using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder;

namespace Slithin.Modules.Settings.UI.ViewModels;

internal class SettingsPageViewModel : BaseViewModel
{
    private readonly LoginInfo _credential;
    private readonly ILogger _logger;
    private readonly ILoginService _loginService;
    private readonly IPathManager _pathManager;
    private readonly SettingsModel _settings;
    private readonly ISettingsService _settingsService;
    private object _settingsContent;

    public SettingsPageViewModel(ILoginService loginService,
        ISettingsService settingsService,
        IPathManager pathManager,
        ILogger logger)
    {
        _credential = loginService.GetCurrentCredential();
        _loginService = loginService;

        FeedbackCommand = new DelegateCommand(Feedback);

        _settingsService = settingsService;
        _pathManager = pathManager;
        _logger = logger;

        _settings = settingsService.GetSettings();

        _credential = _loginService.GetCurrentCredential();
    }

    public bool AutomaticScreenRecovery
    {
        get => _settings.AutomaticScreenRecovery;
        set
        {
            _settings.AutomaticScreenRecovery = value;
            SaveSetting();
        }
    }

    public bool AutomaticTemplateRecovery
    {
        get => _settings.AutomaticTemplateRecovery;
        set
        {
            _settings.AutomaticTemplateRecovery = value;
            SaveSetting();
        }
    }

    public object SettingsContent
    {
        get => _settingsContent;
        set => SetValue(ref _settingsContent, value);
    }

    public string DeviceName
    {
        get => _credential.Name;
        set =>
            new Action<string>((v) =>
            {
                UpdateDeviceName(v);
                OnChange();
            }).Debounce()(value);
    }

    public ICommand FeedbackCommand { get; }

    public bool IsSSHLogin => _loginService.GetCurrentCredential().Key != null;

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
        var oldName = DeviceName;

        if (string.IsNullOrEmpty(newName))
        {
            return;
        }

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

    public override void OnLoad()
    {
        var settingsUiBuilder = ServiceContainer.Current.Resolve<ISettingsUiBuilder>();
        SettingsContent = settingsUiBuilder.Build();
    }
}
