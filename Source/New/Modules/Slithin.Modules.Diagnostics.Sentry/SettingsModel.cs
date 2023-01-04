namespace Slithin.Modules.Diagnostics.Sentry;

internal class SettingsModel
{
    public string Environment { get; set; } = "Debug";
    public bool Debug { get; set; } = true;
    public double TracesSampleRate { get; set; } = 1.0;
    public string DSN { get; set; } = "https://9e8ac6689c704e8493d1753b8788fc57@o207953.ingest.sentry.io/6366736";
}
