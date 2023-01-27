namespace Slithin.Modules.Settings.Models.Builder.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SettingsAttribute : Attribute
{
    public string Label { get; }

    public SettingsAttribute(string label)
    {
        Label = label;
    }
}
