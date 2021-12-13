using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.VPL.Components.Views
{
    public class SwitchView : UserControl
    {
        public SwitchView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
