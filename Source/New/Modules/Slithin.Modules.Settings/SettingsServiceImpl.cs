using AuroraModularis.Core;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Settings;

internal class SettingsServiceImpl : ISettingsService
{
    private readonly ServiceContainer _container;

    public SettingsServiceImpl(ServiceContainer container)
    {
        _container = container;
    }

    public SettingsModel GetSettings()
    {
        var databaseService = _container.Resolve<IDatabaseService>();
        var db = databaseService.GetDatabase();
        var query = db.Query<SettingsModel>();

        if (query.Count() == 1)
        {
            return query.First();
        }

        return new SettingsModel();
    }

    public void Save(SettingsModel settings)
    {
        var databaseService = _container.Resolve<IDatabaseService>();
        var db = databaseService.GetDatabase();

        db.Upsert(settings);
    }
}
