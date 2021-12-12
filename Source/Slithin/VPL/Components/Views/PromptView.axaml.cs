using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.VPL.Components.Views
{
    public class PromptView : UserControl
    {
        public PromptView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
