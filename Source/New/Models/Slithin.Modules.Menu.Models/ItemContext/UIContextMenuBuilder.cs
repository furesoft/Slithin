using AuroraModularis.Core;
using Avalonia.Controls;

namespace Slithin.Modules.Menu.Models.ItemContext;

public static class UIContextMenuBuilder
{
    public static UIContext GetEnable(Control target)
    {
        return UIContext.Notebook;
    }

    public static void SetEnable(Control target, UIContext context)
    {
        void OnTargetOnInitialized(object s, EventArgs _)
        {
            var contextProvider = Container.Current.Resolve<IContextMenuProvider>();

            if (s is ItemsControl ic)
            {
                ic.ContextMenu = contextProvider.BuildMenu(context, ic.DataContext, ic.Parent?.Parent?.DataContext);
            }
            else if (s is Control c)
            {
                if (c.Parent is ListBoxItem lbi)
                {
                    c.ContextMenu = contextProvider.BuildMenu(context, lbi.DataContext, lbi.Parent?.DataContext);
                }
                else
                {
                    c.ContextMenu = contextProvider.BuildMenu(context, c.DataContext, c.Parent?.DataContext);
                }
            }
        }

        target.Initialized += OnTargetOnInitialized;
    }
}
