using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FeatureSwitcher;
using Slithin.Core;
using Slithin.Features;

namespace Slithin.Pages
{
    public partial class MarketPage : UserControl, IPage
    {
        public MarketPage()
        {
            InitializeComponent();
        }

        public string Title => "Market";

        public Control GetContextualMenu() => null;

        bool IPage.IsEnabled()
        {
            return Feature<Market>.Is().Enabled;
        }

        public bool UseContextualMenu() => false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
