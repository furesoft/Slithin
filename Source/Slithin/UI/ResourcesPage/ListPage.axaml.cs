using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.ResourcesPage;

public partial class ListPage : UserControl
{
    public ListPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<ResourceListViewModel>();
    }
}
