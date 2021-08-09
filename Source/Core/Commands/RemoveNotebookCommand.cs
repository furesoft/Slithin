using System;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.ViewModels.Pages;

namespace Slithin.Core.Commands
{
    public class RemoveNotebookCommand : ICommand
    {
        private readonly LocalRepository _localRepository;

        public RemoveNotebookCommand(LocalRepository localRepository)
        {
            _localRepository = localRepository;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter != null && parameter is Metadata md && md.VisibleName != "Quick sheets";
        }

        public async void Execute(object parameter)
        {
            if (parameter is Metadata tmpl)
            {
                if (await DialogService.ShowDialog($"Would you really want to delete '{tmpl.VisibleName}'?"))
                {
                    ServiceLocator.Container.Resolve<NotebooksPageViewModel>().SelectedNotebook = null;
                    ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(tmpl);
                    MetadataStorage.Local.Remove(tmpl);
                    _localRepository.Remove(tmpl);

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
