using Avalonia.Media;
using SlithinMarketplace.Models;

namespace Slithin.Models;

public class Sharable
{
    public AssetModel Asset { get; set; }
    public IImage Image { get; set; }
    public bool IsInstalled { get; set; }
}
