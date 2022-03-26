using LiteDB;

namespace Slithin.Core.Services.Implementations;

public class SettingsService : ISettingsService
{
    private readonly LiteDatabase _db;

    public SettingsService(LiteDatabase db)
    {
        _db = db;
    }

    public Settings GetSettings()
    {
        var collection = _db.GetCollection<Settings>();

        if (collection.Count() == 1)
        {
            return collection.Query().First();
        }

        return new Settings();
    }

    public void Save(Settings settings)
    {
        var collection = _db.GetCollection<Settings>();

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
