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
            // When configuring for the first time, to see what the SDK is doing:
            o.Debug = true;
            // Set traces_sample_rate to 1.0 to capture 100% of transactions for performance monitoring.
            // We recommend adjusting this value in production.
            o.TracesSampleRate = 1.0;
        });

        SentrySdk.CaptureMessage("Hello World");
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
