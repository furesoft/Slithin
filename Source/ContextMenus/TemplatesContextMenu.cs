using System.Collections.Generic;
using Avalonia.Controls;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core;
using Slithin.ViewModels.Pages;

namespace Slithin.ContextMenus
{
    [Context(UIContext.Template)]
    public class TemplatesContextMenu : IContextProvider
    {
        public object ParentViewModel { get; set; }

        public bool CanHandle(object obj)
        {
            return obj is Template;
        }

        public IEnumerable<MenuItem> GetMenu(object obj)
        {
            if (ParentViewModel is TemplatesPageViewModel t)
            {
                yield return new MenuItem { Header = "Remove", Command = new DelegateCommand(_ => t.RemoveTemplateCommand.Execute(obj)) };
            }
        }
    }
}
