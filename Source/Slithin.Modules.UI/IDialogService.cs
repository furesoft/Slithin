using Avalonia.Controls;

namespace Slithin.Modules.UI.Models;

/// <summary>
/// A service to show modals
/// </summary>
public interface IDialogService
{
    Task<bool> Show(string title, Control content);

    /// <summary>
    /// Show modal for asking the user for one value
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    Task<string> ShowPrompt(string title, string message, string defaultValue = "");
}
