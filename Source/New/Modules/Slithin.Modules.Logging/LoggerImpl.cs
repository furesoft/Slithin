using AuroraModularis.Core;
using Serilog;
using Serilog.Core;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Logging;

internal class LoggerImpl : AuroraModularis.Logging.Models.ILogger
{
    private Logger _logger;
    private readonly Container _container;

    private void InitLogger()
    {
        if (_logger != null)
        {
            return;
        }

        var logConfig = new LoggerConfiguration();

        logConfig.WriteTo.File(Path.Combine(_container.Resolve<IPathManager>().SlithinDir, "log.txt"));

#if DEBUG
        logConfig.WriteTo.Debug();
#endif

        _logger = logConfig.CreateLogger();
    }

    public LoggerImpl(Container container)
    {
        _container = container;
    }

    public void Error(string message)
    {
        InitLogger();

        _logger.Error(message);
    }

    public void Info(string message)
    {
        InitLogger();

        _logger.Information(message);
    }
}
