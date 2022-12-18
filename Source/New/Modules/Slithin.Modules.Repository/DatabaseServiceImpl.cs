using AuroraModularis.Core;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

public class DatabaseServiceImpl : IDatabaseService
{
    private Container _container;

    public DatabaseServiceImpl(Container container)
    {
        _container = container;
    }

    public DatabaseAccessor GetDatabase()
    {
        var pathManager = _container.Resolve<IPathManager>();
        return new(new(Path.Combine(pathManager.SlithinDir, "slithin2.db")));
    }
}
