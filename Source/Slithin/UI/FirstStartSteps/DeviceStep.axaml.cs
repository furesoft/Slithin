using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.FirstStartSteps;

public partial class DeviceStep : UserControl
{
    public DeviceStep()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
