using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels;
using Slithin.UI.ContextualMenus;

namespace Slithin.UI.Pages
{
    public partial class NotebooksPage : UserControl, IPage
    {
        public NotebooksPage()
        {
            InitializeComponent();

            DataContext = new NotebooksPageViewModel();
        }

        public string Title => "Notebooks";

        public Control GetContextualMenu()
        {
            return new NotebooksContextualMenu();
        }

        bool IPage.IsEnabled()
        {
            return true;
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
