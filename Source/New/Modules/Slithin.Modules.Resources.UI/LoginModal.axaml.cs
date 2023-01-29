using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Modules.Resources.UI.ViewModels;

namespace Slithin.Modules.Resources.UI;

public partial class LoginModal : UserControl
{
    public LoginModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        if (!Design.IsDesignMode)
        {
            DataContext = ServiceContainer.Current.Resolve<LoginModalViewModel>();
        }
    }
}
