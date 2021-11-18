using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Flyouts;

public class SyncQueueFlyoutViewModel : BaseViewModel
{
    private readonly ILoadingService _loadingService;

    public SyncQueueFlyoutViewModel(ILoadingService loadingService)
    {
        Items = SyncService.SyncQueue;

        DeleteCommand = new DelegateCommand(Delete);
        _loadingService = loadingService;
    }

    public ICommand DeleteCommand { get; set; }

    public ObservableCollection<SyncItem> Items { get; set; } = new();

    private void Delete(object obj)
    {
        var item = (SyncItem)obj;

        SyncService.RemoveFromSyncQueue(item);

        //recover item

        if (item.Data is Metadata md)
        {
            md.Save();

            _loadingService.LoadNotebooks();
        }
        else if (item.Data is Template t)
        {
            TemplateStorage.Instance.Add(t);

            _loadingService.LoadTemplates();
        }
    }
}