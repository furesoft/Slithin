using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.Pages
{
    [Enabled(false)]
    public partial class ToolsPage : UserControl, IPage
    {
        public ToolsPage()
        {
            InitializeComponent();
        }

        public string Title => "Tools";

        public Control GetContextualMenu() => null;

        public bool UseContextualMenu() => false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
