using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Slithin.Modules.Tools.Models;

/// <summary>
/// Interface to represent a custom tool
/// </summary>
public interface ITool
{
    IImage Image { get; }
    ToolInfo Info { get; }

    bool IsConfigurable { get; }

    Control? GetModal();

    void Invoke(object data);
}

public static class IToolExtensions
{
    public static Bitmap LoadImage(this ITool tool, string name)
    {
        return new Bitmap(AssetLoader.Open(new Uri($"avares://{tool.GetType().Namespace}/Resources/{name}")));
    }
}
