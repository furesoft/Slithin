using System;

namespace Slithin.Core.Services;

public interface IVersionService
{
    Version GetDeviceVersion();

    Version GetLocalVersion();

    Version GetSlithinVersion();
}