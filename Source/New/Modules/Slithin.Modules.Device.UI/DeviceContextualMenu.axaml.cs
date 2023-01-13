using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Device.UI;

public partial class DeviceContextualMenu : UserControl
{
    public DeviceContextualMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
