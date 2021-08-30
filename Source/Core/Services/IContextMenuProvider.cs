using Avalonia.Controls;
using Slithin.Core.ItemContext;

namespace Slithin.Core.Services
{
    public interface IContextMenuProvider
    {
        void Add(IContextProvider provider);

        ContextMenu BuildMenu(UIContext context, object item, object parent = null);

        void Init();
    }
}
