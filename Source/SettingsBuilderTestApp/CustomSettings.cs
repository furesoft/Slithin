using Slithin.Core.MVVM;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace SettingsBuilderTestApp;

[DisplaySettings("General Options")]
internal class CustomSettings : BaseViewModel
{
    private bool _autoUpdate = true;
    private string _selectedViewMode;

    [Toggle("Auto Update")]
    public bool AutoUpdate
    {
        get => _autoUpdate;
        set => SetValue(ref _autoUpdate, value);
    }

    [Selection("ViewMode", nameof(SelectedViewMode))]
    public string[] CustomSelectionData { get; set; } = new[] { "BigMenu", "SmallMenu", "Icons" };

    public string SelectedViewMode
    {
        get => _selectedViewMode;
        set => SetValue(ref _selectedViewMode, value);
    }
}
