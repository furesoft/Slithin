using AuroraModularis.Core;
using LiteDB;
using Newtonsoft.Json;

namespace Slithin;

internal class LiteDbSettingsProvider : IModuleSettingsProvider
{
    private LiteDatabase db;

    public LiteDbSettingsProvider()
    {
        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin", "slithin_modules.db");


        var fileInfo = new FileInfo(dbpath);

        if (!fileInfo.Directory.Exists)
        {
            fileInfo.Directory.Create();    
        }
        
        db = new LiteDatabase($"Filename={dbpath}; Connection=Shared");
    }

    public object Load(string path, Type type)
    {
        var settingsCollection = db.GetCollection<BsonDocument>("settings");
        var settingsObj = settingsCollection.FindOne(Query.EQ("$type", type.FullName));

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
        db.BeginTrans();

        var settingsCollection = db.GetCollection<BsonDocument>("settings");

        var settingsObj = settingsCollection.FindOne(Query.EQ("type", data.GetType().FullName));
        var serializedData = JsonConvert.SerializeObject(data);

        if (settingsObj != null)
        {
            settingsObj["obj"] = serializedData;
            settingsCollection.Update(settingsObj);

            db.Commit();
            return;
        }

        settingsObj = new();
        settingsObj.Add("$type", data.GetType().FullName);
        settingsObj.Add("obj", serializedData);
        settingsCollection.Insert(settingsObj);

        db.Commit();
    }
}
