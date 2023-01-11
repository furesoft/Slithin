using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Device.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class PinCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IRemarkableDevice _remarkableDevice;
    private readonly IMetadataRepository _metadataRepository;
    private readonly NotebooksFilter _notebooksFilter;

    public PinCommand(ILocalisationService localisationService,
                      IRemarkableDevice remarkableDevice,
                      IMetadataRepository metadataRepository,
                      NotebooksFilter notebooksFilter)
    {
        _localisationService = localisationService;
        _remarkableDevice = remarkableDevice;
        _metadataRepository = metadataRepository;
        _notebooksFilter = notebooksFilter;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return data is FileSystemModel fsm || fsm.Tag is Metadata md && !md.IsPinned
            && md.VisibleName != _localisationService.GetString("Quick sheets")
            && fsm is not TrashModel;
    }

    public void Execute(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        md.IsPinned = true;
        md.Version++;

        _metadataRepository.SaveToDisk(md);

        _notebooksFilter.Documents.Remove(md);
        _notebooksFilter.Documents.Add(md);

        _notebooksFilter.SortByFolder();

        _metadataRepository.Upload(md);

        _remarkableDevice.Reload();
    }
}
