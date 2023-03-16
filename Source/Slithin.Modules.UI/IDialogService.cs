using Avalonia.Controls;
using Slithin.Controls;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.UI.Models;

/// <summary>
/// A service to show modals
/// </summary>
public interface IDialogService
{
    Task<bool> Show(TranslatedString title, Control content);

    /// <summary>
    /// Show modal for asking the user for one value
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    Task<string> ShowPrompt(TranslatedString title, TranslatedString message, string defaultValue = "");

    /// <summary>
    /// Get the Control where the dialog is containerd. Can be used to place controls on top of the main ui
    /// </summary>
    /// <returns></returns>
    ContentDialog GetContainerControl();
}
