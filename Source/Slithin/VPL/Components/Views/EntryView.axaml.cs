using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.VPL.Components.Views
{
    public class EntryView : UserControl
    {
        public EntryView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}

