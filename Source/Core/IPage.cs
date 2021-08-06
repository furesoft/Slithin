using Avalonia.Controls;

namespace Slithin.Core
{
    public interface IPage
    {
        string Title { get; }

        Control GetContextualMenu();

        bool IsEnabled();

        bool UseContextualMenu();
    }
}
