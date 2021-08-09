using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Flyouts
{
    public class SyncQueueFlyoutViewModel : BaseViewModel
    {
        public SyncQueueFlyoutViewModel()
        {
            foreach (var item in SyncService.SyncQueue.FindAll())
            {
                Items.Add(item);
            }

            DeleteCommand = new DelegateCommand(Delete);
        }

        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<SyncItem> Items { get; set; } = new();

        private void Delete(object obj)
        {
            var item = (SyncItem)obj;

            Items.Remove(item);
            this.SyncService.SyncQueue.Delete(item._id);
        }
    }
}
