using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.Pages
{
    public partial class TemplatesPage : UserControl, IPage
    {
        public TemplatesPage()
        {
            InitializeComponent();
        }

        public string Title => "My Templates";

        public Control GetContextualMenu()
        {
            return new Button() { Content = "Install on Device" };
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