using Avalonia.Controls;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Menu.Models;

/// <summary>
/// A service to build context sensitive context menus
/// </summary>
public interface IContextMenuProvider
{
    void AddProvider(IContextProvider provider);

    ContextMenu BuildMenu<T>(string pageID, T item, object parent = null);

    void Init();
}
