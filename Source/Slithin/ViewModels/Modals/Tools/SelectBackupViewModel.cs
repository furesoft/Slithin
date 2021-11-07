using System.Collections.ObjectModel;
using Slithin.Core;
using Slithin.Models;

namespace Slithin.ViewModels.Modals
{
    public class SelectBackupViewModel : BaseViewModel
    {
        private Backup _selectedBackup;
        public ObservableCollection<Backup> Backups { get; set; } = new();

        public Backup SelectedBackup
        {
            get => _selectedBackup;
            set => SetValue(ref _selectedBackup, value);
        }
    }
}
