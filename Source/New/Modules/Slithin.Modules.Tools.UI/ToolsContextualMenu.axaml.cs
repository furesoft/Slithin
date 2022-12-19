using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Tools.UI;

public partial class ToolsContextualMenu : UserControl
{
    public ToolsContextualMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
