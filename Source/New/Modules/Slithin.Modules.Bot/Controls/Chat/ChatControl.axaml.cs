using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Slithin.Modules.Bot.Controls.Chat;

public partial class ChatControl : UserControl
{
    public ChatControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
