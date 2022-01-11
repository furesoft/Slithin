using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.VPL.Components.Views
{
    public class InvokeToolView : UserControl
    {
        public InvokeToolView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
