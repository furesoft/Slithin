using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.UI.Views
{
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            InitializeComponent();

            DataContext = ServiceLocator.GetLoginCredentials();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
