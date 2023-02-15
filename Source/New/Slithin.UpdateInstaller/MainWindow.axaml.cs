using Avalonia.Controls;
using Slithin.Core.MVVM;
using Slithin.UpdateInstaller.ViewModels;

namespace Slithin.UpdateInstaller;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        BaseViewModel.ApplyViewModel<MainWindowViewModel>(this);
    }
}
