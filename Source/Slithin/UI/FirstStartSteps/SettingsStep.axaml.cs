using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.FirstStartSteps;

public partial class SettingsStep : UserControl
{
    public SettingsStep()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var vm = ServiceLocator.Container.Resolve<SettingsPageViewModel>();
        vm.Load();

        DataContext = vm;
    }
}
