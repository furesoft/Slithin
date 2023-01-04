using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Modules.Menu.Models.Menu;
using Slithin.Modules.Resources.UI.ViewModels;

namespace Slithin.Modules.Resources.UI;

[PreserveIndex(3)]
[PageIcon("Modern.Resource")]
public partial class ResourcesPage : UserControl, IPage
{
    public ResourcesPage()
    {
        InitializeComponent();
    }

    public string Title => "Resources";

    public Control GetContextualMenu() => null;

    bool IPage.IsEnabled()
    {
        return true;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = Container.Current.Resolve<ResourcesPageViewModel>();
    }
}
