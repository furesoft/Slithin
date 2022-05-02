using Amazon.S3;
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
        Storage.UploadObject(bucket, asset.ID, asset);
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
        user.ID = Guid.NewGuid().ToString();

        Storage.UploadObject("users", username, user);
    }

    public UploadRequest CreateUploadRequest(string id)
    {
        return new() { UploadEndpoint = $"/files/upload/{id}" };
    }

    public T GetAsset<T>(string bucket, string id)
    {
        return Storage.GetObject<T>(bucket, id);
    }

    public T[] GetAssets<T>(string bucket)
    {
        var ids = GetIds(bucket);

        return ids.Select(_ => GetAsset<T>(bucket, _)).ToArray();
    }

    public Stream GetFile(string bucket, string id)
    {
        return Storage.GetObjectStream(bucket, id);
    }

    public IEnumerable<string> GetIds(string bucket)
    {
        return Storage.ListObjects(bucket).Select(_ => _.Key);
    }

    public User GetUser(string username)
    {
        return Storage.GetObject<User>("users", username);
    }
}
