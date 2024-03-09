using System.Timers;
using Avalonia.Controls;
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

        _timer = new();
        _timer.Interval = 1000;
        _timer.AutoReset = false;
        _timer.Elapsed += TimerOnElapsed;
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
