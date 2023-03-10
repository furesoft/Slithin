using Avalonia.Media;

namespace Slithin.Modules.Notebooks.UI.Models;

/// <summary>
/// A service to load thumbnails for notebooks/folders
/// </summary>
public interface IThumbnailLoader
{
    IImage LoadImage(FileSystemModel model);
}
