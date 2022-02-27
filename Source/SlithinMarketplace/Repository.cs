using System.Text;
using Amazon.S3;
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

    public void AddScreen(string name, Stream strm)
    {
        Storage.UploadObjectFromStream("screens", name, strm);
    }

    public void AddUser(string username, string password)
    {
        var user = new User();
        user.Username = username;
        user.PasswordHash = Utils.ComputeSha256Hash(password);

        Storage.UploadObjectFromStream("users", username, Serialize(user));
    }

    public User GetUser(string username)
    {
        return Storage.GetObject<User>("users", username);
    }

    private Stream Serialize(object obj)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        var jsonRaw = Encoding.ASCII.GetBytes(json);

        return new MemoryStream(jsonRaw);
    }
}
