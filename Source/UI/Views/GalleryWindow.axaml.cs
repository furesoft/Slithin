using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.UI.Views
{
    public partial class GalleryWindow : Window
    {
        public GalleryWindow(BaseViewModel viewModel)
        {
            DataContext = viewModel;

            InitializeComponent();
        }

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

            ((BaseViewModel)DataContext).OnRequestClose += () => this.Close();
        }
    }
}
