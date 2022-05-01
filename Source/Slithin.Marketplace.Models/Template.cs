using SlithinMarketplace.Models;

namespace Slithin.Marketplace.Models;

public sealed class Template : AssetModel
{
    public string[] categories { get; set; }
    public string filename { get; set; }
    public string iconcode { get; set; }
    public bool islandscape { get; set; }
    public string name { get; set; }
    public string svgfileid { get; set; }
}
