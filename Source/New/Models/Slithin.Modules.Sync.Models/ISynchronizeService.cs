namespace Slithin.Modules.Sync.Models;

/// <summary>
/// A service to synchronize notebooks/screens/templates from device to computer
/// </summary>
public interface ISynchronizeService
{
    /// <summary>
    /// Start the synchronisation process
    /// </summary>
    /// <param name="notificationsInNewWindow">If enabled the modal is shown in new window</param>
    /// <returns></returns>
    Task Synchronize(bool notificationsInNewWindow);
}
