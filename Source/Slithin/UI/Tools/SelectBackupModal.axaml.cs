using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.UI.Tools;

public partial class SelectBackupModal : UserControl
{
    public SelectBackupModal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}