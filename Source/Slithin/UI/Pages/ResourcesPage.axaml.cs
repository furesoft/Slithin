using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.FeatureToggle;
using Slithin.Core.Menu;
using Slithin.Features;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages;

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
        return Feature<ResourcesFeature>.IsEnabled;
    }

    public bool UseContextualMenu() => false;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<ResourcesPageViewModel>();
    }
}
