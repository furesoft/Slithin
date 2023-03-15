using Avalonia.Controls.Notifications;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.UI.Models;

/// <summary>
/// A service to show notifications or a status modal
/// </summary>
public interface INotificationService
{
    void Init(WindowNotificationManager manager);

    void Show(TranslatedString message);

    void ShowError(TranslatedString message);
    void ShowErrorNewWindow(TranslatedString message);

    /// <summary>
    /// Shows a status modal
    /// </summary>
    /// <param name="message"></param>
    /// <param name="isCancellable"></param>
    /// <param name="showInNewWindow"></param>
    /// <returns></returns>
    IStatusController ShowStatus(TranslatedString message, bool isCancellable = false, bool showInNewWindow = false);
}
