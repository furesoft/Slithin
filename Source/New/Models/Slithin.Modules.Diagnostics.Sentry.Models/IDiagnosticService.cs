namespace Slithin.Modules.Diagnostics.Sentry.Models;

/// <summary>
/// A service to send exceptions and minimal performance monitor
/// </summary>
public interface IDiagnosticService
{
    void TrackException(Exception exception);

    IDisposable StartPerformanceMonitoring(string name, string operation);
}
