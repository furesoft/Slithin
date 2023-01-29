using AuroraModularis.Core;
using Sentry;
using Slithin.Modules.Diagnostics.Sentry.Models;

namespace Slithin.Modules.Diagnostics.Sentry;

internal class Module : AuroraModularis.Module
{
    private IDisposable _service;

    public override Task OnStart(ServiceContainer container)
    {
        return Task.CompletedTask;
    }

    public override void RegisterServices(ServiceContainer container)
    {
        var settings = (SettingsModel)Settings;

        void ConfigureOptions(SentryOptions o)
        {
            o.Dsn = settings.DSN;
#if DEBUG
            o.Debug = settings.Debug;
            o.Environment = settings.Environment;
#endif
            o.TracesSampleRate = settings.TracesSampleRate;
        }

        _service = SentrySdk.Init(ConfigureOptions);

        container.Register<IDiagnosticService>(new DiagnosticServiceImpl()).AsSingleton();
        container.Register<IFeedbackService>(new FeedbackServiceImpl()).AsSingleton();
    }

    public override void OnInit()
    {
        UseSettings = true;
        Settings = new SettingsModel();
    }

    public override void OnExit()
    {
        _service?.Dispose();
    }
}
