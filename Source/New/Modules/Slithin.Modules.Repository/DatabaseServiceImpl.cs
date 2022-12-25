using AuroraModularis.Core;
using LiteDB;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

internal class DatabaseServiceImpl : IDatabaseService
{
    internal LiteDatabase db;
    private Container _container;

    public DatabaseServiceImpl(Container container)
    {
        _container = container;
    }

    public DatabaseAccessor GetDatabase()
    {
        if (db == null)
        {
            var pathManager = _container.Resolve<IPathManager>();
            db = new(Path.Combine(pathManager.SlithinDir, "slithin2.db"));
        }

        return new(db);
    }
}
