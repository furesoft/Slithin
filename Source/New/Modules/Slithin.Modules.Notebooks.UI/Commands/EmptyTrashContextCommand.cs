using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Device.Models;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class EmptyTrashCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IRemarkableDevice _remarkableDevice;

    public EmptyTrashCommand(ILocalisationService localisationService,
                                    IRemarkableDevice remarkableDevice)
    {
        _localisationService = localisationService;
        _remarkableDevice = remarkableDevice;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return data is Metadata md && md.VisibleName == _localisationService.GetString("Trash");
    }

    public void Execute(object data)
    {
        /*
        foreach (var trashedMd in MetadataStorage.Local.GetByParent("trash"))
        {
            DeleteNotebook(trashedMd);
        }*/

        _remarkableDevice.Reload();
    }

    /*
    private void DeleteNotebook(Metadata md)
    {
        MetadataStorage.Local.Remove(md);
        _localRepository.Remove(md);
        _deviceRepository.Remove(md);

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();
    }*/
}
