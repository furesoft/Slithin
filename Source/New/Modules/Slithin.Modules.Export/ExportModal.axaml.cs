using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Export;

public partial class ExportModal : UserControl
{
    public ExportModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
