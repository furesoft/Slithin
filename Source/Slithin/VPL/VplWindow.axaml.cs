using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.ViewModels;

namespace Slithin.VPL;

public partial class VplWindow : Window
{
    public VplWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        this.Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, EventArgs e)
    {
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        DataContext = new VplWindowViewModal();
    }
}
