using Avalonia.Controls;

namespace Slithin.Modules.Menu.Models.ContextualMenu;

public interface IContextualMenuBuilder
{
    UserControl BuildContextualMenu(string id);

    void Init();
}
