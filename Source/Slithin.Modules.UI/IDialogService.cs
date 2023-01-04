using Avalonia.Controls;

namespace Slithin.Modules.UI.Models;

public interface IDialogService
{
    Task<bool> Show(string title, Control content);

    Task<string> ShowPrompt(string title, string message, string defaultValue = "");
}
