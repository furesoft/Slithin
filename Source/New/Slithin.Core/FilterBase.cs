using System.Collections.ObjectModel;
using Slithin.Core.MVVM;

namespace Slithin.Core;

public abstract class FilterBase<T> : NotifyObject
{
    private T _selection;

    private ObservableCollection<T> _items = new();

    public event Action<T> SelectionChanged;

    public T Selection
    {
        get => _selection;
        set
        {
            SetValue(ref _selection, value);
            SelectionChanged?.Invoke(value);
        }
    }

    public ObservableCollection<T> Items
    {
        get => _items;
        set => SetValue(ref _items, value);
    }
}
