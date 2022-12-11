using Avalonia.Controls;
using Slithin.Core;

namespace Slithin.Core.Menu;

public interface IPage
{
    string Title { get; }

    Control GetContextualMenu();

    bool IsEnabled();

    bool UseContextualMenu();
}
