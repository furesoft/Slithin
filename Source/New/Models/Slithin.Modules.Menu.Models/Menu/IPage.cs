using Avalonia.Controls;

namespace Slithin.Modules.Menu.Models.Menu;

public interface IPage
{
    string Title { get; }

    Control GetContextualMenu();

    bool IsEnabled();
}
