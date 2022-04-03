using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.FeatureToggle;
using Slithin.ViewModels.Pages;
using Slithin.Core.Menu;
using Slithin.Features;

namespace Slithin.UI.Pages;

[PreserveIndex(3)]
[PageIcon("Marketplace")]
public partial class MarketplacePage : UserControl, IPage
{
    public MarketplacePage()
    {
        InitializeComponent();
    }

    public string Title => "Marketplace";

    public Control GetContextualMenu() => null;

    bool IPage.IsEnabled()
    {
        return Feature<MarketplaceFeature>.IsEnabled;
    }

    public bool UseContextualMenu() => false;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<SharablesPageViewModel>();
    }
}
