using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Notebooks.UI;

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
