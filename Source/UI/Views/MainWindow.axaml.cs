using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Messages;
using Slithin.ViewModels;

namespace Slithin.UI.Views
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

            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            ServiceLocator.Client?.Disconnect();
            ServiceLocator.Scp?.Disconnect();
            ServiceLocator.Database?.Dispose();

            Environment.Exit(0);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            if (!ServiceLocator.Local.GetTemplates().Any() || !Directory.GetFiles(ServiceLocator.NotebooksDir).Any())
            {
                ServiceLocator.Mailbox.Post(new InitStorageMessage());
            }
        }
    }
}
