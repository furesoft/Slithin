using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Slithin.Core;
using Slithin.Tools;

namespace Slithin.ViewModels.Modals
{
    public class SelectBackupViewModel : BaseViewModel
    {
        public ObservableCollection<Backup> Backups { get; set; } = new();

        public Backup SelectedBackup
        {
            get { return _selectedBackup; }
            set { SetValue(ref _selectedBackup, value); }
        }
    }
}
