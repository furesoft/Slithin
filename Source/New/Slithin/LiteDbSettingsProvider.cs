using AuroraModularis.Core;
using LiteDB;
using Newtonsoft.Json;

namespace Slithin;

internal class LiteDbSettingsProvider : IModuleSettingsProvider
{
    public object Load(string path, Type type)
    {
        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin", "slithin2.db");

        var db = new LiteDatabase(dbpath);
        var settingsCollection = db.GetCollection<BsonDocument>("settings");
        var settingsObj = settingsCollection.FindOne(Query.EQ("$type", type.FullName));

        if (settingsObj != null)
        {
            return JsonConvert.DeserializeObject(settingsObj["obj"], type);
        }

        var newInstance = Activator.CreateInstance(type);

        db.Dispose();

        Save(newInstance, path);

        return newInstance;
    }

    public void Save(object data, string path)
    {
        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin", "slithin2.db");

        var db = new LiteDatabase(dbpath);
        var settingsCollection = db.GetCollection<BsonDocument>("settings");

        var settingsObj = settingsCollection.FindOne(Query.EQ("type", data.GetType().FullName));
        var serializedData = JsonConvert.SerializeObject(data);

        if (settingsObj != null)
        {
            settingsObj["obj"] = serializedData;
            settingsCollection.Update(settingsObj);

            db.Dispose();
            return;
        }

        settingsObj = new();
        settingsObj.Add("$type", data.GetType().FullName);
        settingsObj.Add("obj", serializedData);
        settingsCollection.Insert(settingsObj);

        db.Dispose();
    }
}
