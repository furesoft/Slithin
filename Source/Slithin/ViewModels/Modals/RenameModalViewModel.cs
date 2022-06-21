using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Modals;

public class RenameModalViewModel : ModalBaseViewModel
{
    private readonly Metadata _md;
    private readonly SynchronisationService _synchronisationService;

    public RenameModalViewModel(Metadata md)
    {
        RenameCommand = new DelegateCommand(Rename, _ => md != null);
        _synchronisationService = ServiceLocator.SyncService;
        _md = md;
        Name = md?.VisibleName;
    }

    public string Name { get; set; }

    public ICommand RenameCommand { get; set; }

    private void Rename(object obj)
    {
        if (string.IsNullOrEmpty(Name))
        {
            var localisation = ServiceLocator.Container.Resolve<ILocalisationService>();
            DialogService.OpenError(localisation.GetStringFormat("{0} cannot be empty", nameof(Name)));
            return;
        }

        _md.VisibleName = Name;

        MetadataStorage.Local.Remove(_md);
        MetadataStorage.Local.AddMetadata(_md, out var alreadyAdded);

        if (alreadyAdded)
        {
            return;
        }

        _md.Save();

        _md.Upload();

        DialogService.Close();
    }
}
