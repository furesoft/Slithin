using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.UI.ContextualMenus;
using Slithin.ViewModels;

namespace Slithin.UI.Pages
{
    public partial class ToolsPage : UserControl, IPage
    {
        public ToolsPage()
        {
            InitializeComponent();
        }

        public string Title => "Tools";

        public Control GetContextualMenu() => new ToolsContextualMenu();

        bool IPage.IsEnabled()
        {
            return true;
        }

        public bool UseContextualMenu() => true;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = new ToolsPageViewModel();
        }
    }
}
