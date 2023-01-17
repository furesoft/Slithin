using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Menu.Views;

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
