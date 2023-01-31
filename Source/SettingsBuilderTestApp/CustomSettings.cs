using System;
using Avalonia.Layout;
using Slithin.Core.MVVM;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace SettingsBuilderTestApp;

[DisplaySettings("General Options")]
internal class CustomSettings : BaseViewModel
{
    private bool _autoUpdate = true;
    private string _selectedViewMode = string.Empty;
    private string _testString = "";

    [Toggle("Auto Update")]
    public bool AutoUpdate
    {
        get => _autoUpdate;
        set => SetValue(ref _autoUpdate, value);
    }

    [Settings("Custom Date")]
    public DateTime Date { get; set; }

    [Selection("ViewMode", nameof(SelectedViewMode))]
    public string[] CustomSelectionData { get; set; } = {"BigMenu", "SmallMenu", "Icons"};

    public string SelectedViewMode
    {
        get => _selectedViewMode;
        set => SetValue(ref _selectedViewMode, value);
    }

    [Selection("Orientation")] public Orientation Orientation { get; set; }

    [Settings("Some Text")]
    public string TestString
    {
        get => _testString;
        set => SetValue(ref _testString, value);
    }
}
