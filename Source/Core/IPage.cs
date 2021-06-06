using Avalonia.Controls;

namespace Slithin.Core
{
    public interface IPage
    {
        bool UseContextualMenu();
        Control GetContextualMenu();

        string Title { get; }
    }
}