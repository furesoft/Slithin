using AuroraModularis.Core;
using Slithin.Core.MVVM;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder;

namespace Slithin.Modules.Settings.UI.ViewModels;

internal class SettingsPageViewModel : BaseViewModel
{
    private readonly ILoginService _loginService;
    private object _settingsContent;

    public SettingsPageViewModel(ILoginService loginService,
        ISettingsService settingsService)
    {
        loginService.GetCurrentCredential();
        _loginService = loginService;

        settingsService.GetSettings();

        _loginService.GetCurrentCredential();
    }
    
    public object SettingsContent
    {
        get => _settingsContent;
        set => SetValue(ref _settingsContent, value);
    }

    public bool IsSSHLogin => _loginService.GetCurrentCredential().Key != null;

    protected override void OnLoad()
    {
        var settingsUiBuilder = ServiceContainer.Current.Resolve<ISettingsUiBuilder>();
        SettingsContent = settingsUiBuilder.Build();
    }
}
