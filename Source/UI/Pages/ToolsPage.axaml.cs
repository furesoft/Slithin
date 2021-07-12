using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FeatureSwitcher;
using Slithin.Core;
using Slithin.Features;

namespace Slithin.Pages
{
    public partial class ToolsPage : UserControl, IPage
    {
        public ToolsPage()
        {
            InitializeComponent();
        }

        public string Title => "Tools";

        public Control GetContextualMenu() => null;

        bool IPage.IsEnabled()
        {
            return Feature<Tools>.Is().Enabled;
        }

        public bool UseContextualMenu() => false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
