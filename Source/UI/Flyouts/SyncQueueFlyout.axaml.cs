using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels.Flyouts;

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

            this.Initialized += SyncQueueFlyout_Initialized;
        }

        private void SyncQueueFlyout_Initialized(object sender, System.EventArgs e)
        {
            DataContext = ServiceLocator.Container.Resolve<SyncQueueFlyoutViewModel>();
        }
    }
}
