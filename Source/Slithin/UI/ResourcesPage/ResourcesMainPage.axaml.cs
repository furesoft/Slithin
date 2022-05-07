using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.ResourcesPage;

public partial class ResourcesMainPage : UserControl
{
    public ResourcesMainPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<ResourcesPageViewModel>();
    }
}
