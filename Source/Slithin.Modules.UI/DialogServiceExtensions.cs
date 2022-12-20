using Avalonia.Controls;

namespace Slithin.Modules.UI.Models;

public static class DialogServiceExtensions
{
    public static Task<bool> Show(this IDialogService service, string message)
    {
        var tb = new TextBlock();
        tb.MaxWidth = 100;
        tb.TextAlignment = Avalonia.Media.TextAlignment.Left;
        tb.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
        tb.Text = message;

        return service.Show("Information", tb);
    }
}
