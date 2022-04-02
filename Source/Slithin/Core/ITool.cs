using Avalonia.Controls;
using Avalonia.Media;
using Slithin.Models;

namespace Slithin.Core;

public interface ITool
{
    IImage Image { get; }
    ScriptInfo Info { get; }

    bool IsConfigurable { get; }

    Control GetModal();

    void Invoke(object data);
}
