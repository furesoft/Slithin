using Avalonia.Controls.Notifications;

namespace Slithin.Modules.Notifications.Models;

public interface INotificationService
{
    void Init(WindowNotificationManager manager);

    void Show(string message);
}
