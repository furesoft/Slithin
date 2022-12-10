using AuroraModularis.Core;
using LiteDB;
using Newtonsoft.Json;

namespace Slithin;

internal class LiteDbSettingsProvider : IModuleSettingsProvider
{
    private readonly LiteDatabase _db;
    private readonly ILiteCollection<BsonDocument> _settingsCollection;

    public LiteDbSettingsProvider()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin", "slithin2.db");

        _db = new LiteDatabase(path);
        _settingsCollection = _db.GetCollection<BsonDocument>("settings");
    }

    public object Load(string path, Type type)
    {
        var settingsObj = _settingsCollection.FindOne(Query.EQ("$type", type.FullName));

        if (settingsObj != null)
        {
            return JsonConvert.DeserializeObject(settingsObj["obj"], type);
        }

        var newInstance = Activator.CreateInstance(type);

        Save(newInstance, path);

        return newInstance;
    }

    public void Save(object data, string path)
    {
        var settingsObj = _settingsCollection.FindOne(Query.EQ("type", data.GetType().FullName));
        var serializedData = JsonConvert.SerializeObject(data);

        if (settingsObj != null)
        {
            settingsObj["obj"] = serializedData;
            _settingsCollection.Update(settingsObj);
            return;
        }

        settingsObj = new();
        settingsObj.Add("type", data.GetType().FullName);
        settingsObj.Add("obj", serializedData);
        _settingsCollection.Insert(settingsObj);
    }
}
