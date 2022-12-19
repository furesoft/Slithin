using AuroraModularis.Core;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Settings;

public class SettingsServiceImpl : ISettingsService
{
    private readonly Container _container;

    public SettingsServiceImpl(Container container)
    {
        _container = container;
    }

    public SettingsModel GetSettings()
    {
        var databaseService = _container.Resolve<IDatabaseService>();
        var db = databaseService.GetDatabase();
        var collection = db.DB.GetCollection<SettingsModel>();

        if (collection.Count() == 1)
        {
            return collection.Query().First();
        }

        return new SettingsModel();
    }

    public void Save(SettingsModel settings)
    {
        var databaseService = _container.Resolve<IDatabaseService>();
        var db = databaseService.GetDatabase();
        var collection = db.DB.GetCollection<SettingsModel>();

        if (collection.Count() == 1)
        {
            var old = collection.Query().First();
            settings._id = old._id;

            collection.Update(settings);
        }
        else
        {
            collection.Insert(settings);
        }
    }
}
