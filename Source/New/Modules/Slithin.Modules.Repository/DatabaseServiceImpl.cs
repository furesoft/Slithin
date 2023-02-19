using AuroraModularis.Core;
using LiteDB;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Repository;

internal class DatabaseServiceImpl : IDatabaseService
{
    internal LiteDatabase _db;
    private readonly ServiceContainer _container;

    public DatabaseServiceImpl(ServiceContainer container)
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
        _db = new($"Filename={Path.Combine(pathManager.SlithinDir, "slithin.db")}; Connection=Shared");

        return new(_db);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
