using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.FirstStartSteps;
public partial class WelcomeStep : UserControl
{
    public WelcomeStep()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
