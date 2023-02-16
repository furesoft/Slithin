using System;
using Avalonia.Layout;
using Slithin.Modules.Settings.Models;
using Slithin.Modules.Settings.Models.Builder.Attributes;

namespace SettingsBuilderTestApp;

[DisplaySettings("General Options")]
internal class CustomSettings : SavableSettingsModel
{
    private bool _autoUpdate = true;
    private string _selectedViewMode = string.Empty;
    private string _testString = "";
    private TimeOnly _time;
    private DateTime _date;

    [Toggle("Auto Update")]
    public bool AutoUpdate
    {
        get => _autoUpdate;
        set => SetValue(ref _autoUpdate, value);
    }

    [Settings("Custom Date")]
    public DateTime Date
    {
        get => _date;
        set => this.SetValue(ref _date, value);
    }

    [Settings("Custom Time")]
    public TimeOnly Time
    {
        get => _time;
        set => this.SetValue(ref _time, value);
    }

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

    public override void Save()
    {
        
    }
}
