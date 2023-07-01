using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Device.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class EmptyTrashCommand : ICommand
{
    private readonly IMetadataRepository _metadataRepository;
    private readonly NotebooksFilter _notebooksFilter;
    private readonly IRemarkableDevice _remarkableDevice;

    public EmptyTrashCommand(IMetadataRepository metadataRepository,
                             NotebooksFilter notebooksFilter,
                             IRemarkableDevice remarkableDevice)
    {
        _metadataRepository = metadataRepository;
        _notebooksFilter = notebooksFilter;
        _remarkableDevice = remarkableDevice;

        _notebooksFilter.SelectionChanged += (s) =>
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return _notebooksFilter.ParentFolder is TrashModel;
    }

    public void Execute(object data)
    {
        foreach (var trashedMd in _metadataRepository.GetByParent("trash"))
        {
            DeleteNotebook(trashedMd);
        }

        _remarkableDevice.Reload();
    }

    private void DeleteNotebook(Metadata md)
    {
        _metadataRepository.Remove(md);
        // _localRepository.Remove(md);
        //_deviceRepository.Remove(md);

        _notebooksFilter.Items.Remove(_notebooksFilter.Items.First(_ => _.ID == md.ID));
        _notebooksFilter.SortByFolder();
    }
}
