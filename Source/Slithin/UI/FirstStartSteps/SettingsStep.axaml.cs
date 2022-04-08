using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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
    }
}
