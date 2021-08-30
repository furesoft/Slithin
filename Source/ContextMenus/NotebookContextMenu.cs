using System.Collections.Generic;
using Avalonia.Controls;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using System;

using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Controls;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;

using Slithin;
using Slithin.Core;
using Slithin.ViewModels.Pages;

namespace Slithin.ContextMenus
{
    [Context(UIContext.Notebook)]
    public class NotebookContextMenu : IContextProvider
    {
        public object ParentViewModel { get; set; }

        public bool CanHandle(object obj)
        {
            return obj is Metadata;
        }

        public IEnumerable<MenuItem> GetMenu(object obj)
        {
            if (ParentViewModel is NotebooksPageViewModel n)
            {
                yield return new MenuItem { Header = "Copy ID", Command = new DelegateCommand(async _ => await App.Current.Clipboard.SetTextAsync(((Metadata)obj).ID)) };
                yield return new MenuItem { Header = "Remove", Command = new DelegateCommand(_ => n.RemoveNotebookCommand.Execute(obj)) };
                yield return new MenuItem { Header = "Rename", Command = new DelegateCommand(_ => n.RenameCommand.Execute(obj)) };
            }
        }
    }
}
