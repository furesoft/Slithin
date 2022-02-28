using System;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.ViewModels.Pages;

namespace Slithin.Core.Commands;

public class RemoveNotebookCommand : ICommand
{
    private readonly DeviceRepository _deviceRepository;
    private readonly ILocalisationService _localisationService;
    private readonly LocalRepository _localRepository;
    private readonly SynchronisationService _synchronisationService;

    public RemoveNotebookCommand(LocalRepository localRepository,
                                 ILocalisationService localisationService,
                                 DeviceRepository deviceRepository)
    {
        _localRepository = localRepository;
        _localisationService = localisationService;
        _deviceRepository = deviceRepository;
        _synchronisationService = ServiceLocator.SyncService;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && md.VisibleName != _localisationService.GetString("Up ..")
               && md.VisibleName != _localisationService.GetString("Trash");
    }

    public async void Execute(object parameter)
    {
        if (parameter is not Metadata md
            || !await DialogService.ShowDialog(
                _localisationService.GetStringFormat("Would you really want to delete '{0}'?", md.VisibleName)))
            return;

        ServiceLocator.Container.Resolve<NotebooksPageViewModel>().SelectedNotebook = null;

        MetadataStorage.Local.Remove(md);
        _localRepository.Remove(md);
        _deviceRepository.Remove(md);

        _synchronisationService.NotebooksFilter.Documents.Clear();

        foreach (var mds in MetadataStorage.Local.GetByParent(md.Parent))
        {
            ServiceLocator.SyncService.NotebooksFilter.Documents.Add(mds);
        }
        if (md.Parent != "")
        {
            ServiceLocator.SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = "CollectionType", VisibleName = _localisationService.GetString("Up ..") });
        }

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();
    }
}
