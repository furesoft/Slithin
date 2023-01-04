namespace Slithin.Modules.Repository.Models;

public interface IVersionService
{
    Version GetDeviceVersion();

    Version GetLocalVersion();

    Version GetSlithinVersion();

    bool HasLocalVersion();

    void UpdateVersion(Version version);
}
