using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core.MVVM;
using Slithin.Modules.Updater.ViewModels;

namespace Slithin.Modules.Updater;

public partial class UpdaterWindow : Window
{
    public UpdaterWindow()
    {
        InitializeComponent();

        BaseViewModel.ApplyViewModel<UpdaterViewModel>(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        this.AttachDevTools();
    }
}
