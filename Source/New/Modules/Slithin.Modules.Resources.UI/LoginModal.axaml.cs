using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core.MVVM;
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

        BaseViewModel.ApplyViewModel<LoginModalViewModel>(this);
    }
}
