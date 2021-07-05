using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.Pages
{
    [Enabled(false)]
    public partial class DevicePage : UserControl, IPage
    {
        public DevicePage()
        {
            InitializeComponent();
        }

        public string Title => "My Device";

        public Control GetContextualMenu()
        {
            return null;
        }

        public bool UseContextualMenu()
        {
            return false;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
