using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Device.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class UnPinCommand : ICommand
{
    private readonly IRemarkableDevice _remarkableDevice;

    public UnPinCommand(IRemarkableDevice remarkableDevice)
    {
        _remarkableDevice = remarkableDevice;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return data is Metadata md;// && md.IsPinned;
    }

    public void Execute(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }
        /*
        md.IsPinned = false;
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
