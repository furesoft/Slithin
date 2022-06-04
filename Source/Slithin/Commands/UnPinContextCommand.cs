using System;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Commands;

public class UnPinCommand : ICommand
{
    private readonly Xochitl _xochitl;

    public UnPinCommand(Xochitl xochitl)
    {
        _xochitl = xochitl;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object data)
    {
        return data is Metadata md && md.IsPinned;
    }

    public void Execute(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        md.IsPinned = false;
        md.Version++;
        md.Save();

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

        md.Upload();

        _xochitl.ReloadDevice();
    }
}
