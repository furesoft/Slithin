using SlithinMarketplace.Models;

namespace Slithin.Marketplace.Models;

public sealed class Template : AssetModel
{
    public string[] Categories { get; set; }
    public bool IsLandscape { get; set; }
    public string Name { get; set; }
}
