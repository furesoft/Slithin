using BlobHelper;
using Newtonsoft.Json;

namespace SlithinMarketplace;

public class ServiceLocator
{
    public static void s()
    {
        var keys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("keys.json"));

        AwsSettings settings = new AwsSettings(
        keys["Url"],      // http://localhost:8000/
        false,          // enable or disable SSL
        keys["AccessKey"],
        keys["SecretKey"],
        AwsRegion.USEast1,
        "screens",
        keys["Url"]        // i.e. http://localhost:8000/{bucket}/{key}
        );

        Blobs blobs = new Blobs(settings);
        var s = blobs.Enumerate().Result;
        //Repository = new Repository(client);
    }
}
