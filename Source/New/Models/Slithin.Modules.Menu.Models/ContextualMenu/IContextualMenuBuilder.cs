using Avalonia.Controls;

namespace Slithin.Modules.Menu.Models.ContextualMenu;

/// <summary>
/// A service to build header submenu
/// </summary>
public interface IContextualMenuBuilder
{
    UserControl BuildContextualMenu(string id);

    void Init();
}
