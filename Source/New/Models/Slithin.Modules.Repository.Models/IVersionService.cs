namespace Slithin.Modules.Repository.Models;

/// <summary>
/// A service to work with slithin/device versions. Used for device update detection
/// </summary>
public interface IVersionService
{
    Version GetDeviceVersion();

    Version GetLocalVersion();

    Version GetSlithinVersion();

    bool HasDeviceUpdated();

    bool HasLocalVersion();

    void UpdateVersion(Version version);
}
