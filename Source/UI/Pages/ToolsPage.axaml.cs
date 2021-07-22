using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.UI.Pages
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
            return false;
        }

        public bool UseContextualMenu() => false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
