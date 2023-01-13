using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Menu.Models.Menu;
using Slithin.Modules.Tools.UI.ViewModels;

namespace Slithin.Modules.Tools.UI;

[PreserveIndex(4)]
[PageIcon("FeatherIcons.Tool")]
[Context(UIContext.Tools)]
public partial class ToolsPage : UserControl, IPage
{
    public ToolsPage()
    {
        InitializeComponent();
    }

    public string Title => "Tools";
    
    bool IPage.IsEnabled()
    {
        return true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = Container.Current.Resolve<ToolsPageViewModel>();
    }
}
