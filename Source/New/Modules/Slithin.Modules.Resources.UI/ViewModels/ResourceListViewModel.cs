using System.Collections.ObjectModel;
using Slithin.Core.MVVM;
using Slithin.Modules.Resources.Models;

namespace Slithin.Modules.Resources.UI.ViewModels;

public class ResourceListViewModel : BaseViewModel
{
    private ObservableCollection<Sharable> _items;

    public ObservableCollection<Sharable> Items
    {
        get { return _items; }
        set { SetValue(ref _items, value); }
    }
}
