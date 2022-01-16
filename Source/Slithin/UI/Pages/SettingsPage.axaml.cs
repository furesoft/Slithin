using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Features;
using Slithin.Core.FeatureToggle;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages;

public partial class SettingsPage : UserControl, IPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    public string Title => "Settings";

    public Control GetContextualMenu() => null;

    bool IPage.IsEnabled()
    {
        return Feature<SettingsFeature>.IsEnabled;
    }

    public bool UseContextualMenu() => false;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<SettingsPageViewModel>();
    }
}
