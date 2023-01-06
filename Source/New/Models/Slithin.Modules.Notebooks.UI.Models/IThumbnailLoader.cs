using Avalonia.Media;

namespace Slithin.Modules.Notebooks.UI.Models;

public interface IThumbnailLoader
{
    IImage LoadImage(FilesystemModel model);
}
