using AuroraModularis.Core;
using LiteDB;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

internal class DatabaseServiceImpl : IDatabaseService
{
    internal LiteDatabase _db;
    private readonly Container _container;

    public DatabaseServiceImpl(Container container)
    {
        _container = container;
    }

    public DatabaseAccessor GetDatabase()
    {
        if (_db != null)
        {
            return new(_db);
        }

        var pathManager = _container.Resolve<IPathManager>();
        _db = new($"Filename={Path.Combine(pathManager.SlithinDir, "slithin2.db")}; Connection=Shared");

        return new(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
