using System.Collections.ObjectModel;
using Slithin.Core.MVVM;
using Slithin.Models;

namespace Slithin.ViewModels.Pages;

public class ResourceListViewModel : BaseViewModel
{
    private ObservableCollection<Sharable> _items;

    public ObservableCollection<Sharable> Items
    {
        get { return _items; }
        set { SetValue(ref _items, value); }
    }
}
