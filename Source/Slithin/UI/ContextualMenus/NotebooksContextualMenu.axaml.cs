using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.ContextualMenus;

public partial class NotebooksContextualMenu : UserControl
{
    public NotebooksContextualMenu()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
