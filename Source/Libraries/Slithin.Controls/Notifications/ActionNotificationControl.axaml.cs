using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Controls.Notifications;

public partial class ActionNotificationControl : UserControl
{
    public ActionNotificationControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
