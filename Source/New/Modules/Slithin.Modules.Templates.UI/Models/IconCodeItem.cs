using Avalonia.Media;

namespace Slithin.Modules.Templates.UI.Models;

public class IconCodeItem
{
    public IImage Image { get; set; }
    public string Name { get; set; }

    public override string ToString() => Name;
}
