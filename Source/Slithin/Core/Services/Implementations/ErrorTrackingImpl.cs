using System;
using Sentry;

namespace Slithin.Core.Services.Implementations;

public class ErrorTrackingImpl : IErrorTrackingService
{
    private IDisposable _service;

    public void Dispose()
    {
        _service.Dispose();
    }

    public void Init()
    {
        _service = SentrySdk.Init(o =>
        {
            o.Dsn = "https://9e8ac6689c704e8493d1753b8788fc57@o207953.ingest.sentry.io/6366736";

#if DEBUG
            o.Debug = true;
#endif
            o.TracesSampleRate = 1.0;
        });
    }

    public IDisposable StartPerformanceMonitoring(string name, string operation)
    {
        return new PerformanceMonitor(name, operation);
    }

    public void TrackException(Exception ex)
    {
        SentrySdk.CaptureException(ex);
    }

    public class PerformanceMonitor : IDisposable
    {
        private ISpan _span;
        private ITransaction _transaction;

        public PerformanceMonitor(string name, string operation)
        {
            _transaction = SentrySdk.StartTransaction(name, operation);
            _span = _transaction.StartChild(operation);
        }

        public void Dispose()
        {
            _transaction.Finish();
            _span.Finish();
        }
    }
}
