using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Device.UI;

public partial class DefaultContextualMenu : UserControl
{
    public DefaultContextualMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
