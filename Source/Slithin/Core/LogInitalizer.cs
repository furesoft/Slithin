using System.IO;
using Serilog;
using Slithin.Core.Services;

namespace Slithin.Core;

public class LogInitalizer
{
    private readonly IPathManager _pathManager;

    public LogInitalizer(IPathManager pathManager)
    {
        _pathManager = pathManager;
    }

    public void Init()
    {
        var log = new LoggerConfiguration()
            .WriteTo.Debug()
            .WriteTo.File(Path.Combine(_pathManager.ConfigBaseDir, "log.txt"))
            .CreateLogger();

        ServiceLocator.Container.Register<ILogger>(log);
    }
}
