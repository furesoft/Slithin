using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.UI.Modals;

public partial class StatusModal : UserControl
{
    public StatusModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
