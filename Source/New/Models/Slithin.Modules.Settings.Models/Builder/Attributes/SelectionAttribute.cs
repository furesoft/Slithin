namespace Slithin.Modules.Settings.Models.Builder.Attributes;

public class SelectionAttribute : SettingsBaseAttribute
{
    public string? SelectionPropertyName { get; }

    public SelectionAttribute(string label, string? selectionPropertyName = null) : base(label)
    {
        SelectionPropertyName = selectionPropertyName;
    }
}
