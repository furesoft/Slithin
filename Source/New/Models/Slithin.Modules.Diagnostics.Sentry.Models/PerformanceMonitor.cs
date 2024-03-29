﻿using Sentry;

namespace Slithin.Modules.Diagnostics.Sentry.Models;

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
