using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.UI.Modals;

public partial class ShowDialogModal : UserControl
{
    public ShowDialogModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
