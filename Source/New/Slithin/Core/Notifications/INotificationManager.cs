using System.Threading.Tasks;

namespace Slithin.Core.Notifications;

/// <summary>
/// Represents a notification manager that can be used to show notifications in a window or using
/// the host operating system.
/// </summary>
public interface INotificationManager
{
    /// <summary>
    /// Show a notification.
    /// </summary>
    /// <param name="notification">The notification to be displayed.</param>
    Task<Avalonia.Controls.Notifications.NotificationCard> Show(INotification notification);
}
