using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace Slithin.Modules.Settings.UI.SettingModels;

[DisplaySettings("Appeareance", IsExpanded = true)]
public class AppeareanceSettingsModel : SavableSettingsModel
{
    private readonly ISettingsService _settingsService;
    private readonly SettingsModel _settings;
    private bool _isBigMenuMode = true;
    private bool _isDarkMode;

    public AppeareanceSettingsModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _settings = settingsService.GetSettings();

        IsBigMenuMode = _settings.IsBigMenuMode;
        IsDarkMode = _settings.IsDarkMode;
    }

    [Toggle("Big Menu Mode")]
    public bool IsBigMenuMode
    {
        get => _isBigMenuMode;
        set => this.SetValue(ref _isBigMenuMode, value);
    }

    [Toggle("Dark Mode")]
    public bool IsDarkMode
    {
        get => _isDarkMode;
        set => this.SetValue(ref _isDarkMode, value);
    }

    public override void Save()
    {
        _settings.IsDarkMode = IsDarkMode;
        _settings.IsBigMenuMode = IsBigMenuMode;
        
        _settingsService.Save(_settings);
    }
}
