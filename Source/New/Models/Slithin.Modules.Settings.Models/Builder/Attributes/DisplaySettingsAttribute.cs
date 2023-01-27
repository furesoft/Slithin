namespace Slithin.Modules.Settings.Models.Builder.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DisplaySettingsAttribute : SettingsBaseAttribute
{
    public DisplaySettingsAttribute(string label)
        : base(label)
    {
        
    }
}
