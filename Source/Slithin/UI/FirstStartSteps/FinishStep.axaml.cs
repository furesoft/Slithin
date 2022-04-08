using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.FirstStartSteps;
public partial class FinishStep : UserControl
{
    public FinishStep()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
