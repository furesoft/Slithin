using Amazon.S3;

namespace SlithinMarketplace;

public class ServiceLocator
{
    static ServiceLocator()
    {
        var client = new AmazonS3Client();
        Repository = new Repository(client);
    }

    public static Repository Repository { get; set; }
}
