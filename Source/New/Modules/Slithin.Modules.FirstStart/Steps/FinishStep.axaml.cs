using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.FirstStart.Steps;
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
