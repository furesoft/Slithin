using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.UI.FirstStartSteps;

public partial class DeviceStep : UserControl
{
    public DeviceStep()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        var vm = ServiceLocator.Container.Resolve<AddDeviceWindowViewModel>();
        vm.Load();

        DataContext = vm;
    }
}
