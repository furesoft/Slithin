namespace Slithin.Modules.Settings.Models.Builder.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SettingsBaseAttribute : Attribute
{
    public string Label { get; }

    public SettingsBaseAttribute(string label)
    {
        Label = label;
    }
}
