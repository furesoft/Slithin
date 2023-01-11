using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Device.Models;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class UnPinCommand : ICommand
{
    private readonly IRemarkableDevice _remarkableDevice;
    private readonly IMetadataRepository _metadataRepository;
    private readonly NotebooksFilter _notebooksFilter;

    public UnPinCommand(IRemarkableDevice remarkableDevice,
                        IMetadataRepository metadataRepository,
                        NotebooksFilter notebooksFilter)
    {
        _remarkableDevice = remarkableDevice;
        _metadataRepository = metadataRepository;
        _notebooksFilter = notebooksFilter;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return data is FileSystemModel fsm && fsm.Tag is Metadata md && md.IsPinned;
    }

    public void Execute(object data)
    {
        if (data is not FileSystemModel fsm || fsm.Tag is not Metadata md)
        {
            return;
        }

        md.IsPinned = false;
        md.Version++;
        _metadataRepository.SaveToDisk(md);

        _notebooksFilter.Documents.Remove(fsm);
        _notebooksFilter.Documents.Add(fsm);

        _notebooksFilter.SortByFolder();

        _metadataRepository.Upload(md);

        _remarkableDevice.Reload();
    }
}
