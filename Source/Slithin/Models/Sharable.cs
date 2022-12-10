using Avalonia.Media;
using Slithin.Core.MVVM;
using Slithin.Marketplace.Models;

namespace Slithin.Models;

public class Sharable : NotifyObject
{
    private IImage _image;
    public AssetModel Asset { get; set; }

    public IImage Image
    {
        get { return _image; }
        set { SetValue(ref _image, value); }
    }

    public bool IsInstalled { get; set; }
}
