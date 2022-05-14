using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using Slithin.Marketplace.Models;
using SlithinMarketplace.Models;

namespace Slithin.API.Lib;

public class MarketplaceAPI
{
    private RestClient _client;
    private string _token;

    public MarketplaceAPI()
    {
        _client = new RestClient("https://slithin-api.multiplayer.co.at/api");
        _client.UseNewtonsoftJson();
    }

    public async void Authenticate(string username, string password)
    {
        var request = new RestRequest("/token", Method.Post)
            .AddBody(new Grant { GrantType = "password", Username = username, Password = password });

        var cts = new CancellationTokenSource();
        var result = _client.PostAsync<AuthenticationResult>(request, cts.Token).Result;

        if (result != null)
        {
            _token = result.AccessToken;
            _client.Authenticator = new JwtAuthenticator(_token);

            Console.WriteLine("Login Successful");
        }
    }

    public async void CreateAndUploadAsset(object? assetObj, string fileToUpload)
    {
        var request = new RestRequest($"/{assetObj.GetType().Name.ToLower()}s", Method.Put);
        request.AddBody(assetObj);

        var result = _client.PutAsync<UploadRequest>(request).Result;

        var wc = new WebClient();
        wc.Headers.Add("Authorization", "Bearer " + _token);

        var ms = new MemoryStream();
        using (var fs = File.OpenRead(fileToUpload))
        {
            fs.CopyTo(ms);
        }

        wc.UploadData(_client.BuildUri(new RestRequest(result.UploadEndpoint)), ms.ToArray());
    }

    public void CreateAndUploadTemplate(Template? template, string templateInfoPath)
    {
        var templateBasePath = new FileInfo(templateInfoPath).Directory.FullName;

        var svgPath = Path.Combine(templateBasePath, template.Filename + ".svg");
        var pngPath = Path.Combine(templateBasePath, template.Filename + ".png");

        CreateAndUploadAsset(template, pngPath);

        UploadFile(template.SvgFileID, svgPath);
    }

    public async void DownloadFile(string id, string filename)
    {
        var data = GetBytes(id);
        File.WriteAllBytes(filename, data);
    }

    public T Get<T>(string bucket, int count = 0, int skip = 0)
    {
        var request = new RestRequest($"/{bucket}", Method.Get);

        if (count != 0)
        {
            request.AddQueryParameter("count", count);
        }
        if (skip != 0)
        {
            request.AddQueryParameter("skip", skip);
        }

        return _client.GetAsync<T>(request).Result;
    }

    public byte[] GetBytes(string fileID)
    {
        var wc = new WebClient();
        wc.Headers.Add("Authorization", "Bearer " + _token);

        return wc.DownloadData(_client.BuildUri(new RestRequest($"/files/{fileID}")));
    }

    public async void UploadFile(string id, string fileToUpload)
    {
        var request = new RestRequest($"/files/request/{id}", Method.Get);

        var result = _client.GetAsync<UploadRequest>(request).Result;

        var wc = new WebClient();
        wc.Headers.Add("Authorization", "Bearer " + _token);

        var ms = new MemoryStream();
        using (var fs = File.OpenRead(fileToUpload))
        {
            fs.CopyTo(ms);
        }

        wc.UploadData(_client.BuildUri(new RestRequest(result.UploadEndpoint)), ms.ToArray());
    }
}
