using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.Views;
public partial class FirstStartWindow : Window
{
    public FirstStartWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
