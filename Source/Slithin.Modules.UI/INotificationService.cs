using Avalonia.Controls.Notifications;

namespace Slithin.Modules.UI.Models;

/// <summary>
/// A service to show notifications or a status modal
/// </summary>
public interface INotificationService
{
    void Init(WindowNotificationManager manager);

    void Show(string message);

    void ShowError(string message);
    void ShowErrorNewWindow(string message);

    /// <summary>
    /// Shows a status modal
    /// </summary>
    /// <param name="message"></param>
    /// <param name="isCancellable"></param>
    /// <param name="showInNewWindow"></param>
    /// <returns></returns>
    IStatusController ShowStatus(string message, bool isCancellable = false, bool showInNewWindow = false);
}
