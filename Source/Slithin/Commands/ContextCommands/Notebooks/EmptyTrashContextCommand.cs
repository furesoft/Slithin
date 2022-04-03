using Slithin.Core;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;

namespace Slithin.Commands.ContextCommands.Notebooks;

[Context(UIContext.Notebook)]
public class EmptyTrashContextCommand : IContextCommand
{
    private readonly DeviceRepository _deviceRepository;
    private readonly ILocalisationService _localisationService;
    private readonly LocalRepository _localRepository;
    private readonly Xochitl _xochitl;

    public EmptyTrashContextCommand(ILocalisationService localisationService,
                                    LocalRepository localRepository,
                                    DeviceRepository deviceRepository,
                                    Xochitl xochitl)
    {
        _localisationService = localisationService;
        _localRepository = localRepository;
        _deviceRepository = deviceRepository;
        _xochitl = xochitl;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Empty Trash");

    public bool CanHandle(object data)
    {
        return data is Metadata md && md.VisibleName == _localisationService.GetString("Trash");
    }

    public void Invoke(object data)
    {
        foreach (var trashedMd in MetadataStorage.Local.GetByParent("trash")) //may parallelize
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
