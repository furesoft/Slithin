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
        private readonly SynchronisationService _synchronisationService;

        public RemoveNotebookCommand(LocalRepository localRepository)
        {
            _localRepository = localRepository;
            _synchronisationService = ServiceLocator.SyncService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter != null
                && parameter is Metadata md
                && md.VisibleName != "Quick sheets"
                && md.VisibleName != "Up ..";
        }

        public async void Execute(object parameter)
        {
            if (parameter is not Metadata md
                || !await DialogService.ShowDialog($"Would you really want to delete '{md.VisibleName}'?"))
                return;

            ServiceLocator.Container.Resolve<NotebooksPageViewModel>().SelectedNotebook = null;
            _synchronisationService.NotebooksFilter.Documents.Clear();
            MetadataStorage.Local.Remove(md);
            _localRepository.Remove(md);

            var item = new SyncItem
            {
                Action = SyncAction.Remove,
                Direction = SyncDirection.ToDevice,
                Data = md,
                Type = SyncType.Notebook
            };

            _synchronisationService.AddToSyncQueue(item);

            foreach (var mds in MetadataStorage.Local.GetByParent(md.Parent))
            {
                ServiceLocator.SyncService.NotebooksFilter.Documents.Add(mds);
            }
            if (md.Parent != "")
            {
                ServiceLocator.SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = "CollectionType", VisibleName = "Up .." });
            }

            ServiceLocator.SyncService.NotebooksFilter.SortByFolder();
        }
    }
}
