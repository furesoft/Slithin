using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Modules.FirstStart.ViewModels;

namespace Slithin.Modules.FirstStart;

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

        var vm = Container.Current.Resolve<FirstStartViewModel>();
        vm.Load();

        vm.OnRequestClose += () => Close();

        DataContext = vm;
    }
}
