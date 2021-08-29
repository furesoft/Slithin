using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.GalleryFirstStart
{
    public partial class WelcomePage : UserControl
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
