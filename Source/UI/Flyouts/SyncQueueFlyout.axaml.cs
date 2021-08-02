using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.ViewModels;

namespace Slithin.UI.Flyouts
{
    public partial class SyncQueueFlyout : UserControl
    {
        public SyncQueueFlyout()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = new SyncQueueFlyoutViewModel();
        }
    }
}
