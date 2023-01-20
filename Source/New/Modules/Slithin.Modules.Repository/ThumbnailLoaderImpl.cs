using AuroraModularis.Core;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Entities.Remarkable;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Notebooks.UI.Models;

namespace Slithin.Modules.Repository;

internal class ThumbnailLoaderImpl : IThumbnailLoader
{
    public IImage? LoadImage(FileSystemModel model)
    {
        if (model.Tag is not Metadata md)
        {
            return null;
        }

        var notebooksDir = Container.Current.Resolve<IPathManager>()!.NotebooksDir;
        var cache = Container.Current.Resolve<ICacheService>();
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        if (!Directory.Exists(Path.Combine(notebooksDir, md.ID + ".thumbnails")))
        {
            return cache.GetObject("notebook-" + md.Content.FileType,
                () => new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png"))));
        }

        var filename = "";

        if (md.Content.Pages == null)
        {
            return null;
        }
            
        if (md.Content.CoverPageNumber == 0)
        {
            // load first page
            filename = md.Content.Pages[0];
        }
        else if (md.Content.CoverPageNumber == -1 )
        {
            // load last page opened, set in md.LastOpenedPage
            filename = md.Content.Pages[md.LastOpenedPage];
        }

        if (string.IsNullOrEmpty(filename))
        {
            return cache.GetObject("notebook-" + md.Content.FileType,
                () => new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png"))));
        }

        var thumbnail = Path.Combine(notebooksDir, $"{md.ID}.thumbnails", $"{filename}.jpg");

        if (File.Exists(thumbnail))
        {
            return cache.GetObject(thumbnail, () => new Bitmap(File.OpenRead(thumbnail)));
        }

        return cache.GetObject("notebook-" + md.Content.FileType,
           () => new Bitmap(assets!.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png"))));
    }
}
