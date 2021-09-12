using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.Views
{
    public partial class AddDeviceWindow : Window
    {
        public AddDeviceWindow()
        {
            InitializeComponent();
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
