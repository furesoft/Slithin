using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Device.Models;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

public class PinCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IRemarkableDevice _remarkableDevice;

    public PinCommand(ILocalisationService localisationService, IRemarkableDevice remarkableDevice)
    {
        _localisationService = localisationService;
        _remarkableDevice = remarkableDevice;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return data is Metadata md //&& !md.IsPinned
            && md.VisibleName != _localisationService.GetString("Quick sheets")
            && md.VisibleName != _localisationService.GetString("Trash");
    }

    public void Execute(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        /*
        md.IsPinned = true;
        md.Version++;
        md.Save();

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

        md.Upload();

        */
        _remarkableDevice.Reload();
    }
}
