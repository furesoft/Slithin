using Amazon.S3;

namespace SlithinMarketplace;

public class ServiceLocator
{
    static ServiceLocator()
    {
        AmazonS3Config config = new AmazonS3Config();
        config.ServiceURL = Environment.GetEnvironmentVariable("MINIO_HOST");
        config.ForcePathStyle = true;
        config.AuthenticationRegion = AuthenticationRegion.USEast1.SystemName;

        AmazonS3Client client = new(
            Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY"),
            Environment.GetEnvironmentVariable("MINIO_SECRET_KEY"), config);

        Repository = new Repository(client);
    }

    public static Repository Repository { get; set; }
}
