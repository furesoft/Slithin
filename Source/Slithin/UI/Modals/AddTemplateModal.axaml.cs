using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.Modals
{
    public partial class AddTemplateModal : UserControl
    {
        public AddTemplateModal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
