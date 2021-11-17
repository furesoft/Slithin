using Avalonia.Controls;
using Slithin.Core.Services;

namespace Slithin.Core.ItemContext;

public class UIContextMenuBuilder
{
    public static UIContext GetEnable(Control target)
    {
        return UIContext.Notebook;
    }

    public static void SetEnable(Control target, UIContext context)
    {
        target.Initialized += (s, e) =>
        {
            var contextProvider = ServiceLocator.Container.Resolve<IContextMenuProvider>();

            if (s is ItemsControl ic)
            {
                ic.ContextMenu = contextProvider.BuildMenu(context, ic.DataContext, ic.Parent.Parent.DataContext);
            }
            else if (s is Control c)
            {
                c.ContextMenu = contextProvider.BuildMenu(context, c.DataContext, c.Parent.DataContext);
            }
        };
    }
}