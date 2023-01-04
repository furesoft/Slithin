using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Resources.UI.Pages;

public partial class RegisterFramePage : UserControl
{
    public RegisterFramePage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
