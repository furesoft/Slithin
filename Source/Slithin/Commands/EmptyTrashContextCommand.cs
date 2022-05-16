using System;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;

namespace Slithin.Commands;

public class EmptyTrashCommand : ICommand
{
    private readonly DeviceRepository _deviceRepository;
    private readonly ILocalisationService _localisationService;
    private readonly LocalRepository _localRepository;
    private readonly Xochitl _xochitl;

    public EmptyTrashCommand(ILocalisationService localisationService,
                                    LocalRepository localRepository,
                                    DeviceRepository deviceRepository,
                                    Xochitl xochitl)
    {
        _localisationService = localisationService;
        _localRepository = localRepository;
        _deviceRepository = deviceRepository;
        _xochitl = xochitl;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return data is Metadata md && md.VisibleName == _localisationService.GetString("Trash");
    }

    public void Execute(object data)
    {
        foreach (var trashedMd in MetadataStorage.Local.GetByParent("trash"))
        {
            DeleteNotebook(trashedMd);
        }

        _xochitl.ReloadDevice();
    }

    private void DeleteNotebook(Metadata md)
    {
        MetadataStorage.Local.Remove(md);
        _localRepository.Remove(md);
        _deviceRepository.Remove(md);

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();
    }
}
