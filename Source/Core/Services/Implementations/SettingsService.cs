using System.Linq;
using LiteDB;

namespace Slithin.Core.Services.Implementations
{
    public class SettingsService : ISettingsService
    {
        private readonly LiteDatabase _db;

        public SettingsService(LiteDatabase db)
        {
            _db = db;
        }

        public Settings Get()
        {
            var collection = _db.GetCollection<Settings>();

            if (collection.Count() == 1)
            {
                return collection.FindAll().First();
            }
            else
            {
                return new();
            }
        }

        public void Save(Settings settings)
        {
            var collection = _db.GetCollection<Settings>();

            if (collection.Count() == 1)
            {
                var old = collection.FindAll().First();
                settings._id = old._id;

                collection.Update(settings);
            }
            else
            {
                collection.Insert(settings);
            }
        }
    }
}
