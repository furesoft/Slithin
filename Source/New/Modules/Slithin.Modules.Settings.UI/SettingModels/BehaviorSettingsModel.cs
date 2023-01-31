using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.UI.SettingModels;

[DisplaySettings("Automatic Updates")]
public class BehaviorSettingsModel : SavableSettingsModel
{
    private readonly ISettingsService _settingsService;
    private readonly SettingsModel _settings;
    private bool _automaticTemplateRecovery;
    private bool _automaticScreenRecovery;

    public BehaviorSettingsModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _settings = settingsService.GetSettings();

        AutomaticScreenRecovery = _settings.AutomaticScreenRecovery;
        AutomaticTemplateRecovery = _settings.AutomaticTemplateRecovery;
    }
    
    [Toggle("Automatic Screen Recovery")]
    public bool AutomaticScreenRecovery
    {
        get => _automaticScreenRecovery;
        set => this.SetValue(ref _automaticScreenRecovery, value);
    }

    [Toggle("Automatic Template Recovery")]
    public bool AutomaticTemplateRecovery
    {
        get => _automaticTemplateRecovery;
        set => this.SetValue(ref _automaticTemplateRecovery, value);
    }

    public override void Save()
    {
        _settings.AutomaticScreenRecovery = AutomaticScreenRecovery;
        _settings.AutomaticTemplateRecovery = AutomaticTemplateRecovery;
        
        _settingsService.Save(_settings);
    }
}
