namespace Slithin.Modules.Settings.Models.Builder.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DisplaySettingsAttribute : SettingsAttribute
{
    public DisplaySettingsAttribute(string label)
        : base(label)
    {
    }

    public bool IsExpanded { get; set; }
}
