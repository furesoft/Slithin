using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Sync;

namespace Slithin.ViewModels
{
    public class SyncQueueFlyoutViewModel : BaseViewModel
    {
        private SyncItem _selectedItem;

        public SyncQueueFlyoutViewModel()
        {
            Items.Add(new SyncItem { Action = SyncAction.Add, Direction = SyncDirection.ToDevice, Type = SyncType.Template, Data = "Something" });
            Items.Add(new SyncItem { Action = SyncAction.Add, Direction = SyncDirection.ToDevice, Type = SyncType.Template, Data = "Something" });

            DeleteCommand = new DelegateCommand(Delete);
        }

        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<SyncItem> Items { get; set; } = new();

        public SyncItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetValue(ref _selectedItem, value); }
        }

        private void Delete(object obj)
        {
            Items.Remove(SelectedItem);
        }
    }
}
