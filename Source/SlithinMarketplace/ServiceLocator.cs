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

        MongoClient = new MongoClient("mongodb://slithin:7b6gzDPSnkzEJJvF0CAd2FtjY9OJsjyhuSnIV4mN@mongodb");
        Database = MongoClient.GetDatabase("slithin");
        Repository = new Repository(client);
    }

    public static Repository Repository { get; set; }
}
