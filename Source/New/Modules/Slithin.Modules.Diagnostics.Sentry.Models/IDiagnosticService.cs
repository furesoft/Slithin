namespace Slithin.Modules.Diagnostics.Sentry.Models;

public interface IDiagnosticService
{
    void TrackException(Exception exception);

    IDisposable StartPerformanceMonitoring(string name, string operation);
}
