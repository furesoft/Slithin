using Avalonia.Controls;

namespace Slithin.Core
{
    public interface ITool
    {
        ScriptInfo Info { get; }

        Control GetModal();

        void Invoke(object data);
    }
}
