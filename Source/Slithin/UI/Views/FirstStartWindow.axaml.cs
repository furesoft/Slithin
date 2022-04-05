using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.UI.Views;

public partial class FirstStartWindow : Window
{
    public FirstStartWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var vm = ServiceLocator.Container.Resolve<FirstStartViewModel>();
        vm.Load();

        DataContext = vm;
    }
}
