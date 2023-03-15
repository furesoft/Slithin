using Avalonia.Controls;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.UI.Models;

public static class DialogServiceExtensions
{
    public static Task<bool> Show(this IDialogService service, TranslatedString message)
    {
        var tb = new TextBlock();
        tb.MaxWidth = 100;
        tb.TextAlignment = Avalonia.Media.TextAlignment.Left;
        tb.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
        tb.Text = message;

        return service.Show("Information", tb);
    }
}
