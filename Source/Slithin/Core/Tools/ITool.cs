using Avalonia.Controls;
using Avalonia.Media;
using Slithin.Models;
using Slithin.Core;

namespace Slithin.Core.Tools;

public interface ITool
{
    IImage Image { get; }
    ScriptInfo Info { get; }

    bool IsConfigurable { get; }

    Control GetModal();

    void Invoke(object data);
}
