using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.UI.Pages
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
            return false;
        }

        public bool UseContextualMenu() => false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
