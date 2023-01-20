using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.TemplateEditor;

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
