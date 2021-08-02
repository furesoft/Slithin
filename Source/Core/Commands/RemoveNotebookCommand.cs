using System;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.ViewModels;

namespace Slithin.Core.Commands
{
    public class RemoveNotebookCommand : ICommand
    {
        private readonly NotebooksPageViewModel _notebooksPageViewModel;

        public RemoveNotebookCommand(NotebooksPageViewModel templatesPageViewModel)
        {
            _notebooksPageViewModel = templatesPageViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter != null && parameter is Metadata;
        }

        public async void Execute(object parameter)
        {
            if (parameter is Metadata tmpl)
            {
                if (await DialogService.ShowDialog($"Would you really want to delete '{tmpl.VisibleName}'?"))
                {
                    _notebooksPageViewModel.SelectedNotebook = null;
                    ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(tmpl);
                    MetadataStorage.Local.Remove(tmpl);
                    ServiceLocator.Local.Remove(tmpl);

                    var item = new SyncItem
                    {
                        Action = SyncAction.Remove,
                        Direction = SyncDirection.ToDevice,
                        Data = tmpl,
                        Type = SyncType.Notebook
                    };

                    ServiceLocator.SyncService.SyncQueue.Insert(item);
                }
            }
        }
    }
}
