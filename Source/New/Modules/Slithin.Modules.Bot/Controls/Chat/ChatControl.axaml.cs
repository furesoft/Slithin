using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Timer = System.Timers.Timer;

namespace Slithin.Modules.Bot.Controls.Chat;

public partial class ChatControl : UserControl
{
    private Timer _timer;

    public ChatControl()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        ((ObservableCollection<ChatMessage>)this.FindControl<ItemsPresenter>("chatList").Items).CollectionChanged +=
            OnCollectionChanged;
        this.FindControl<ItemsPresenter>("chatList").ItemContainerGenerator.Materialized +=
            ItemContainerGeneratorOnMaterialized;

        _timer = new System.Timers.Timer();
        _timer.Interval = 1000;
        _timer.AutoReset = false;
        _timer.Elapsed += TimerOnElapsed;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ScrollToEnd();
    }

    private void ItemContainerGeneratorOnMaterialized(object? sender, ItemContainerEventArgs e)
    {
        ScrollToEnd();
    }

    private void ScrollToEnd()
    {
        _timer.Start();
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            this.FindControl<ScrollViewer>("chatScrollViewer").ScrollToEnd();
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
