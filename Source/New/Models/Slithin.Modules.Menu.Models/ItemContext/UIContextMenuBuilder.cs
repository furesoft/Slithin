using AuroraModularis.Core;
using Avalonia.Controls;

namespace Slithin.Modules.Menu.Models.ItemContext;

public static class UIContextMenuBuilder
{
    public static string GetEnable(Control target)
    {
        return "Notebook";
    }

    public static void SetEnable(Control target, string pageID)
    {
        void OnTargetOnInitialized(object s, EventArgs _)
        {
            var contextProvider = ServiceContainer.Current.Resolve<IContextMenuProvider>();

            if (s is ItemsControl ic)
            {
                ic.ContextMenu = contextProvider.BuildMenu(pageID, ic.DataContext, ic.Parent?.Parent?.DataContext);
            }
            else if (s is Control c)
            {
                if (c.Parent is ListBoxItem lbi)
                {
                    c.ContextMenu = contextProvider.BuildMenu(pageID, lbi.DataContext, lbi.Parent?.DataContext);
                }
                else
                {
                    c.ContextMenu = contextProvider.BuildMenu(pageID, c.DataContext, c.Parent?.DataContext);
                }
            }
        }

        target.Initialized += OnTargetOnInitialized;
    }
}
