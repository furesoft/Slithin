using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Controls.Notifications;

public partial class StatusNotificationControl : UserControl
{
    public StatusNotificationControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
