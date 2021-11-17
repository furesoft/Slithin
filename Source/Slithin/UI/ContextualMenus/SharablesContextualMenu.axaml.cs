using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.ContextualMenus;

public class SharablesContextualMenu : UserControl
{
    public SharablesContextualMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
