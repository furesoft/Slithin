using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Modules.Resources.UI.ViewModels;

namespace Slithin.Modules.Resources.UI.Pages;

public partial class ListPage : UserControl
{
    public ListPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceContainer.Current.Resolve<ResourceListViewModel>();
    }
}
