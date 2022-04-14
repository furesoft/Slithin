using Amazon.S3;
using Newtonsoft.Json;

namespace SlithinMarketplace;

public class ServiceLocator
{
    static ServiceLocator()
    {
        var keys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("keys.json"));

        var config = new AmazonS3Config
        {
            //AuthenticationRegion = RegionEndpoint.USEast1.SystemName, // Should match the `MINIO_REGION` environment variable.
            ServiceURL = keys["Url"],
            ForcePathStyle = true // MUST be true to work correctly with MinIO server
        };

        var client = new AmazonS3Client(keys["AccessKey"], keys["SecretKey"], config);
        Repository = new Repository(client);
    }

    public static Repository Repository { get; set; }
}
