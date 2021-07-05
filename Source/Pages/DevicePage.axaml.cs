using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FeatureSwitcher;
using Slithin.Core;
using Slithin.Features;

namespace Slithin.Pages
{
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

        bool IPage.IsEnabled()
        {
            return Feature<Device>.Is().Enabled;
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
