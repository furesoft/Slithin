using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.ContextualMenus;

public partial class EmptyContextualMenu : UserControl
{
    public EmptyContextualMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
