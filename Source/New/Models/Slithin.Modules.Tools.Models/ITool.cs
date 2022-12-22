using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Slithin.Modules.Tools.Models;

public interface ITool
{
    IImage Image { get; }
    ToolInfo Info { get; }

    bool IsConfigurable { get; }

    Control GetModal();

    void Invoke(object data);
}

public static class IToolExtensions
{
    public static Bitmap LoadImage(this ITool tool, string name)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        return new Bitmap(assets.Open(new Uri($"avares://{tool.GetType().Namespace}/Resources/backup.png")));
    }
}
