using SlithinMarketplace.Models;

namespace Slithin.Marketplace.Models;

public sealed class Template : AssetModel
{
    public string[] Categories { get; set; }
    public string Filename { get; set; }
    public string IconCode { get; set; }
    public bool IsLandscape { get; set; }
    public string Name { get; set; }
    public string SvgFileID { get; set; }
}
