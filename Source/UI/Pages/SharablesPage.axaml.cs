using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages
{
    public partial class SharablesPage : UserControl, IPage
    {
        public SharablesPage()
        {
            InitializeComponent();
        }

        public string Title => "Sharables";

        public Control GetContextualMenu() => null;

        bool IPage.IsEnabled()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }

        public bool UseContextualMenu() => false;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = ServiceLocator.Container.Resolve<SharablesPageViewModel>();
        }
    }
}
