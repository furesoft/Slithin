using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core.MVVM;
using Slithin.Modules.Settings.UI.ViewModels;

namespace Slithin.Modules.Settings.UI;

public partial class FeedbackWindow : Window
{
    public FeedbackWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        BaseViewModel.ApplyViewModel<FeedbackViewModel>(this);
    }
}
