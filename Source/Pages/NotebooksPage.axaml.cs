using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels;

namespace Slithin.Pages
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
            return null;
        }

        bool IPage.IsEnabled()
        {
            return true;
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
