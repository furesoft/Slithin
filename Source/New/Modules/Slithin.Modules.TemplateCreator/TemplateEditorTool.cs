using Avalonia.Controls;
using Avalonia.Media;
using Slithin.Modules.Tools.Models;

namespace Slithin.Modules.TemplateCreator;

public class TemplateEditorTool : ITool
{
    public IImage Image { get; }

    public ToolInfo Info => new("templateeditor", "Template Creator", "Templates", "Create custom Templates", false,
        false);
    public bool IsConfigurable => false;
    public Control? GetModal()
    {
        return null;
    }

    public void Invoke(object data)
    {
    }
}
