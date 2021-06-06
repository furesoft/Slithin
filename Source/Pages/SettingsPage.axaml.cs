using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Slithin.Core;

namespace Slithin.Pages
{
    public partial class SettingsPage : UserControl, IPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public string Title => "Settings";

        public Control GetContextualMenu()
        {
            return new Button() { Content = "Add Something" };
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