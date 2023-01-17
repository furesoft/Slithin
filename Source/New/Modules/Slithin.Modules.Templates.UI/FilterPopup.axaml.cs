using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Templates.UI;

public partial class FilterPopup : UserControl
{
    public FilterPopup()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

