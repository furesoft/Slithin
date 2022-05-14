using System;

namespace Slithin.Core.Services;

public interface IErrorTrackingService
{
    void Dispose();

    void Init();

    IDisposable StartPerformanceMonitoring(string name, string operation);

    void TrackException(Exception ex);
}
