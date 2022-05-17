using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.UI.Views;

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

        var feedbackViewModel = ServiceLocator.Container.Resolve<FeedbackViewModel>();
        DataContext = feedbackViewModel;

        feedbackViewModel.OnRequestClose += () => this.Close();
    }
}
