using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Modules.FirstStart.ViewModels;

namespace Slithin.Modules.FirstStart.Steps;

public partial class DeviceStep : UserControl
{
    public DeviceStep()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = ServiceContainer.Current.Resolve<DeviceStepViewModel>();
    }
}
