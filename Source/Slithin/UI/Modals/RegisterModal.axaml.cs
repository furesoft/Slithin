using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels.Modals;

namespace Slithin.UI.Modals;

public partial class RegisterModal : UserControl
{
    public RegisterModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceLocator.Container.Resolve<RegisterModalViewModel>();
    }
}
