using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.ViewModels;

namespace Slithin.UI.Views
{
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            InitializeComponent();

#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var li = ServiceLocator.Container.Resolve<ILoginService>().GetLoginCredentials();
            var cvm = ServiceLocator.Container.Resolve<ConnectionWindowViewModel>();

            cvm.LoginCredentials = new(li);

            DataContext = cvm;
        }
    }
}
