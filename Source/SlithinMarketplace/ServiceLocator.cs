using Amazon.S3;
using Newtonsoft.Json;

namespace SlithinMarketplace;

public class ServiceLocator
{
    public static void s()
    {
        var keys = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("keys.json"));

        AmazonS3Config config = new AmazonS3Config();
        config.ServiceURL = keys["Url"];

        Amazon.S3.AmazonS3Client client = new(keys["AccessKey"], keys["SecretKey"], config);
        var wrapper = new S3Wrapper(client);

        var buckets = wrapper.ListObjects("screens");

        //Repository = new Repository(client);
    }
}
