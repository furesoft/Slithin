using System.Collections.Generic;
using Avalonia.Controls;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core;
using Slithin.ViewModels.Pages;
using Slithin.Core.Sync;

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
                yield return new MenuItem { Header = "Move to Trash", Command = new DelegateCommand(_ => MoveToTrash(obj)) };
            }
        }

        private void MoveToTrash(object obj)
        {
            if (obj is Metadata md)
            {
                MetadataStorage.Local.Move(md, "trash");

                var syncItem = new SyncItem
                {
                    Action = SyncAction.Update,
                    Direction = SyncDirection.ToDevice,
                    Data = md,
                    Type = SyncType.Notebook
                };

                ServiceLocator.SyncService.AddToSyncQueue(syncItem);
            }
        }
    }
}
