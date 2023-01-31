using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Settings.UI.Modals;

public partial class SupportModal : UserControl
{
    public SupportModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

