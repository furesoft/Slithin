using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.Modals;

public partial class ErrorModal : UserControl
{
    public ErrorModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}