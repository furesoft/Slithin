using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Modals;

public class RenameModalViewModel : BaseViewModel
{
    private readonly Metadata _md;
    private readonly SynchronisationService _synchronisationService;
    private string _name;

    public RenameModalViewModel(Metadata md)
    {
        RenameCommand = new DelegateCommand(Rename, (_) => md != null);
        _synchronisationService = ServiceLocator.SyncService;
        _md = md;
        Name = md?.VisibleName;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public ICommand RenameCommand { get; set; }

    private void Rename(object obj)
    {
        if (string.IsNullOrEmpty(Name))
        {
            DialogService.OpenDialogError("Name cannot be empty");
            return;
        }
        _md.VisibleName = Name;

        MetadataStorage.Local.Remove(_md);
        MetadataStorage.Local.Add(_md, out var alreadyAdded);

        if (alreadyAdded)
            return;

        _md.Save();

        var syncItem = new SyncItem
        {
            Action = SyncAction.Update,
            Data = _md,
            Direction = SyncDirection.ToDevice,
            Type = SyncType.Notebook
        };

        _synchronisationService.AddToSyncQueue(syncItem);

        DialogService.Close();
    }
}