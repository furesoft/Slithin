using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.ContextualMenus;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.Pages
{
    public partial class TemplatesPage : UserControl, IPage
    {
        public TemplatesPage()
        {
            InitializeComponent();

            DataContext = new TemplatesPageViewModel();
        }

        public string Title => "My Templates";

        public Control GetContextualMenu()
        {
            return new TemplatesContextualMenu();
        }

        public bool UseContextualMenu()
        {
            return true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
