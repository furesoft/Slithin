using Avalonia.Controls;
using Slithin.Core.ItemContext;

namespace Slithin.Core.Services;

public interface IContextMenuProvider
{
    void AddProvider(IContextProvider provider);

    ContextMenu BuildMenu<T>(UIContext context, T item, object parent = null);

    void Init();
}
