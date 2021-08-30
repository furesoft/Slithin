using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Slithin.Core.Services;

namespace Slithin.Core.ItemContext
{
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
                if (s is Control c)
                {
                    var contextProvider = ServiceLocator.Container.Resolve<IContextMenuProvider>();

                    c.ContextMenu = contextProvider.BuildMenu(context, c.DataContext, c.Parent.Parent.DataContext);
                }
            };
        }
    }
}
