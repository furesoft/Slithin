using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Settings.UI.Modals;

public partial class AboutModal : UserControl
{
    public AboutModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

