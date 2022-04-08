using Avalonia.Interactivity;

namespace Slithin.Controls.Ports;

public class RoutedEventArgsOfT<T> : RoutedEventArgs
{
    public RoutedEventArgsOfT(T info)
    {
        Info = info;
    }

    public RoutedEventArgsOfT(RoutedEvent routedEvent, IInteractive source) : base(routedEvent, source)
    {
    }

    public T Info { get; set; }
}
