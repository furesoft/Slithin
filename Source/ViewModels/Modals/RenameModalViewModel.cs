using System.Windows.Input;
using Material.Styles;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Modals
{
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
            get { return _name; }
            set { _name = value; }
        }

        public ICommand RenameCommand { get; set; }

        private void Rename(object obj)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                _md.VisibleName = Name;

                MetadataStorage.Local.Remove(_md);
                MetadataStorage.Local.Add(_md, out var alreadyAdded);

                if (!alreadyAdded)
                {
                    _md.Save();

                    var syncItem = new SyncItem
                    {
                        Action = SyncAction.Update,
                        Data = _md,
                        Direction = SyncDirection.ToDevice,
                        Type = SyncType.Notebook
                    };

                    _synchronisationService.SyncQueue.Insert(syncItem);

                    DialogService.Close();
                }
            }
            else
            {
                SnackbarHost.Post("Name cannot be empty");
            }
        }
    }
}
