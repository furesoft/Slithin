using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.Pages
{
    [Enabled(false)]
    public partial class MarketPage : UserControl, IPage
    {
        public MarketPage()
        {
            InitializeComponent();
        }

        public string Title => "Market";

        public Control GetContextualMenu() => null;

        public bool UseContextualMenu() => false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
