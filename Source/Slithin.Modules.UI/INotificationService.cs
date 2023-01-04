using Avalonia.Controls.Notifications;

namespace Slithin.Modules.UI.Models;

public interface INotificationService
{
    void Init(WindowNotificationManager manager);

    void Show(string message);

    IStatusController ShowStatus(string message, bool showInNewWindow = false);
}
