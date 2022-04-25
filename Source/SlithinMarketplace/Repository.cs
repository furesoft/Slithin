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

    public void AddFile(string id, Stream stream)
    {
        Storage.UploadObjectFromStream("files", id, stream);
    }

    public void AddScreen(Screen screen)
    {
        Storage.UploadObject("screens", screen.ID, screen);
    }

    public void AddUser(string username, string password)
    {
        var user = new User();
        user.Username = username;
        user.HashedPassword = Utils.ComputeSha256Hash(password);

        Storage.UploadObject("users", username, user);
    }

    public UploadRequest CreateUploadRequest(string id)
    {
        return new() { UploadEndpoint = $"/files/upload/{id}" };
    }

    public Stream GetFile(string bucket, string id)
    {
        return Storage.GetObjectStream(bucket, id);
    }

    public Screen GetScreen(string id)
    {
        return Storage.GetObject<Screen>("screens", id);
    }

    public IEnumerable<string> GetScreenIds()
    {
        return Storage.ListObjects("screens").Select(_ => _.Key);
    }

    public Screen[] GetScreens()
    {
        var ids = GetScreenIds();

        return ids.Select(_ => GetScreen(_)).ToArray();
    }

    public User GetUser(string username)
    {
        return Storage.GetObject<User>("users", username);
    }
}
