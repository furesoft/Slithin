using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.TemplateCreator;

public class TemplateEditorControl : UserControl
{
    public TemplateEditorControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
