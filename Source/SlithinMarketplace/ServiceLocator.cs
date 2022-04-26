using Amazon.S3;
using Newtonsoft.Json;

namespace SlithinMarketplace;

public class ServiceLocator
{
    static ServiceLocator()
    {
        var keys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("keys.json"));

        AmazonS3Config config = new AmazonS3Config();
        config.ServiceURL = keys["Url"];
        config.ForcePathStyle = true;
        config.AuthenticationRegion = "us-east1";

        AmazonS3Client client = new(keys["AccessKey"], keys["SecretKey"], config);

        Repository = new Repository(client);
    }

    public static Repository Repository { get; set; }
}
