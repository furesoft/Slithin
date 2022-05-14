using Amazon;
using Amazon.S3;
using MongoDB.Driver;

namespace SlithinMarketplace;

public class ServiceLocator
{
    public static IMongoDatabase Database;
    public static MongoClient MongoClient;

    static ServiceLocator()
    {
        AmazonS3Config config = new AmazonS3Config();
        config.ServiceURL = Environment.GetEnvironmentVariable("MINIO_HOST");
        config.ForcePathStyle = true;
        config.AuthenticationRegion = RegionEndpoint.USEast1.SystemName;

        AmazonS3Client client = new(
            Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY"),
            Environment.GetEnvironmentVariable("MINIO_SECRET_KEY"), config);

        MongoClient = new MongoClient($"mongodb://{Environment.GetEnvironmentVariable("MONGODB_USERNAME")}:{Environment.GetEnvironmentVariable("MONGODB_PASSWORD")}@{Environment.GetEnvironmentVariable("MONGODB_HOST")}:{Environment.GetEnvironmentVariable("MONGODB_PORT")}");
        Database = MongoClient.GetDatabase("slithin");
        Repository = new Repository(client);
    }

    public static Repository Repository { get; set; }
}
