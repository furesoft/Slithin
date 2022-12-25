using System.Collections.ObjectModel;
using Slithin.Core.MVVM;
using Slithin.Modules.Backup.Models;

namespace Slithin.Modules.Backup.ViewModels;

internal class SelectBackupViewModel : ModalBaseViewModel
{
    private BackupModel _selectedBackup;
    public ObservableCollection<BackupModel> Backups { get; set; } = new();

    public BackupModel SelectedBackup
    {
        get => _selectedBackup;
        set => SetValue(ref _selectedBackup, value);
    }
}
