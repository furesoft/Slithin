using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Modules.Tools.Models;

public interface ITool
{
    IImage Image { get; }
    ToolInfo Info { get; }

    bool IsConfigurable { get; }

    Control GetModal();

    void Invoke(object data);
}
