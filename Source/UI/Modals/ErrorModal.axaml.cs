using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modals
{
    public partial class ErrorModal : UserControl
    {
        public ErrorModal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
