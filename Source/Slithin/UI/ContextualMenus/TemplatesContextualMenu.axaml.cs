using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.ContextualMenus;

public partial class TemplatesContextualMenu : UserControl
{
    public TemplatesContextualMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}