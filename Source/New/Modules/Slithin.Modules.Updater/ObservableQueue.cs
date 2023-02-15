using System.Collections.Specialized;
using System.ComponentModel;

namespace Slithin.Modules.Updater;

public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    public ObservableQueue()
    {
    }

    public ObservableQueue(IEnumerable<T> collection)
    {
        foreach (var item in collection)
        {
            base.Enqueue(item);
        }
    }

    public ObservableQueue(List<T> list)
    {
        foreach (var item in list)
        {
            base.Enqueue(item);
        }
    }

    public new virtual void Clear()
    {
        base.Clear();
        OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
    }

    public new virtual T Dequeue()
    {
        var item = base.Dequeue();
        OnCollectionChanged(new(NotifyCollectionChangedAction.Remove, item));
        return item;
    }

    public new virtual void Enqueue(T item)
    {
        base.Enqueue(item);
        OnCollectionChanged(new(NotifyCollectionChangedAction.Add, item));
    }

    public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        RaiseCollectionChanged(e);
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        RaisePropertyChanged(e);
    }

    protected virtual event PropertyChangedEventHandler PropertyChanged;

    private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (CollectionChanged != null)
        {
            CollectionChanged(this, e);
        }
    }

    private void RaisePropertyChanged(PropertyChangedEventArgs e)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, e);
        }
    }

    event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
    {
        add => PropertyChanged += value;
        remove => PropertyChanged -= value;
    }
}
