using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Features;
using Slithin.Core.FeatureToggle;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages;

[PreserveIndex(3)]
public partial class SharablesPage : UserControl, IPage
{
    public SharablesPage()
    {
        InitializeComponent();
    }

    public string Title => "Sharables";

    public Control GetContextualMenu() => null;

    bool IPage.IsEnabled()
    {
        return Feature<SharableFeature>.IsEnabled;
    }

    public bool UseContextualMenu() => false;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<SharablesPageViewModel>();
    }
}
