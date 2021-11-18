using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.Modals;

public partial class RenameModal : UserControl
{
    public RenameModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}