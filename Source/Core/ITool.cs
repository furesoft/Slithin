using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Core
{
    public interface ITool
    {
        IImage Image { get; }
        ScriptInfo Info { get; }

        bool IsConfigurable { get; }

        Control GetModal();

        void Invoke(object data);
    }
}
