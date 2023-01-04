using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.FirstStart.Steps;

public partial class SettingsStep : UserControl
{
    public SettingsStep()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
