using System;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;

namespace Slithin.Commands;

public class PinCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly Xochitl _xochitl;

    public PinCommand(ILocalisationService localisationService, Xochitl xochitl)
    {
        _localisationService = localisationService;
        _xochitl = xochitl;
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Pin");

    public bool CanExecute(object data)
    {
        return data is Metadata md && !md.IsPinned
            && md.VisibleName != _localisationService.GetString("Quick sheets")
            && md.VisibleName != _localisationService.GetString("Trash");
    }

    public void Execute(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        md.IsPinned = true;
        md.Version++;
        md.Save();

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

        md.Upload();

        _xochitl.ReloadDevice();
    }
}
