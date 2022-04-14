﻿using Amazon.S3;
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

    public void AddScreen(Screen screen, Stream strm)
    {
        Storage.UploadObject("screens", screen.ID, screen);
        Storage.UploadObjectFromStream("files", screen.ID, strm);
    }

    public void AddUser(string username, string password)
    {
        var user = new User();
        user.Username = username;
        user.PasswordHash = Utils.ComputeSha256Hash(password);

        Storage.UploadObject("users", username, user);
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

    public string GetScreens(int count)
    {
        var ids = GetScreenIds().Take(count);

        return Serialize(ids.Select(_ => GetScreen(_)));
    }

    public User GetUser(string username)
    {
        return Storage.GetObject<User>("users", username);
    }

    public string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}
