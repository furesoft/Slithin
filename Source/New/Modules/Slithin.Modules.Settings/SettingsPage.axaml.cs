using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Menu.Models.Menu;
using Slithin.Modules.Settings.ViewModels;

namespace Slithin.Modules.Settings;

[PreserveIndex(5)]
[PageIcon("Vaadin.CogOutline")]
[Context(UIContext.Settings)]
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
            DataContext = Container.Current.Resolve<SettingsPageViewModel>();
        }
    }
}
