using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Templates.UI;

public partial class AddTemplateModal : UserControl
{
    public AddTemplateModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
