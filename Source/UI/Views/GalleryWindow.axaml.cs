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

            var vm = new GalleryWindowViewModel();
            vm.Slides.Add(new GalleryFirstStart.WelcomePage());

            DataContext = vm;
            ((BaseViewModel)DataContext).OnRequestClose += () => this.Close();
        }
    }
}
