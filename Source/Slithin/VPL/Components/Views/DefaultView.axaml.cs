using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.VPL.Components.Views
{
    public class DefaultView : UserControl
    {
        public DefaultView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
