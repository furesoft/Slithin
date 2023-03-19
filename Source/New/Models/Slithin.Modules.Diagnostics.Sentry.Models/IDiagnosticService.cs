namespace Slithin.Modules.Diagnostics.Sentry.Models;

/// <summary>
/// A service to send exceptions and minimal performance monitor
/// </summary>
public interface IDiagnosticService
{
    /// <summary>
    /// Send Exception to tracking service
    /// </summary>
    /// <param name="exception"></param>
    void TrackException(Exception exception);

    /// <summary>
    /// Start a performance monitor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="operation"></param>
    /// <returns></returns>
    IDisposable StartPerformanceMonitoring(string name, string operation);
}
