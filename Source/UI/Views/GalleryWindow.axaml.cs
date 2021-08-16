using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.UI.Views
{
    public partial class GalleryWindow : Window
    {
        public GalleryWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = ServiceLocator.Container.Resolve<GalleryWindowViewModel>();
            ((BaseViewModel)DataContext).OnRequestClose += () => this.Close();
        }
    }
}
