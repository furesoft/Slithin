using SlithinMarketplace.Models;

namespace Slithin.Marketplace.Models;

public sealed class Template : AssetModel
{
    public string[] Categories { get; set; }
    public string filename { get; set; }
    public string iconcode { get; set; }
    public bool isLandscape { get; set; }
    public string name { get; set; }
    public string SvgFileID { get; set; }
}
