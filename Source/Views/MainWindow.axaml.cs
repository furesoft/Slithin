using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new MainWindowViewModel();
            ((MainWindowViewModel)DataContext).LoadMetadataCommand.Execute(null);

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
#if !DEBUG
            ServiceLocator.Client.Disconnect();
            ServiceLocator.Scp.Disconnect();
            ServiceLocator.Database.Dispose();

            Environment.Exit(0);
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}