using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Menu;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages;

[PreserveIndex(5)]
[PageIcon("Vaadin.CogOutline")]
public partial class SettingsPage : UserControl, IPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    public string Title => "Settings";

    public Control GetContextualMenu() => null;

    bool IPage.IsEnabled() => true;

    public bool UseContextualMenu() => false;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        if (!Design.IsDesignMode)
        {
            DataContext = ServiceLocator.Container.Resolve<SettingsPageViewModel>();
        }
    }
}
