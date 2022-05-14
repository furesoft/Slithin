using Amazon.S3;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SlithinMarketplace.Models;

namespace SlithinMarketplace;

public class Repository
{
    public Repository(AmazonS3Client client)
    {
        Storage = new S3Wrapper(client);
    }

    public S3Wrapper Storage { get; set; }

    public void AddAsset(string bucket, AssetModel asset)
    {
        ServiceLocator.Database.GetCollection<AssetModel>(bucket).InsertOne(asset);
    }

    public void AddFile(string id, Stream stream)
    {
        Storage.UploadObjectFromStream("files", id, stream);
    }

    public void AddUser(string username, string password)
    {
        var user = new User();
        user.Username = username;
        user.HashedPassword = Utils.ComputeSha256Hash(password);
        user.Role = "User";

        ServiceLocator.Database.GetCollection<User>("users").InsertOne(user);
    }

    public UploadRequest CreateUploadRequest(string id)
    {
        return new() { UploadEndpoint = $"/files/upload/{id}" };
    }

    public async Task<T> GetAsset<T>(string bucket, ObjectId id)
        where T : AssetModel
    {
        var filter = Builders<T>.Filter;

        var asset = await ServiceLocator.Database.GetCollection<T>(bucket)
            .FindAsync(filter.Eq(_ => _.ID, id));

        return asset.First();
    }

    public List<T> GetAssets<T>(string bucket)
        where T : AssetModel
    {
        return ServiceLocator.Database.GetCollection<T>(bucket).Find(Builders<T>.Filter.Empty).ToList();
    }

    public Stream GetFile(string bucket, string id)
    {
        return Storage.GetObjectStream(bucket, id);
    }

    public User GetUser(string username)
    {
        Console.WriteLine($"'{username}'");
        var user = ServiceLocator.Database.GetCollection<User>("users")
            .Find(Builders<User>.Filter.Eq(_ => _.Username, username)).Single();

        Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));

        return user;
    }
}
