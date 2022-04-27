using System.Net;
using ApiConsole.Core;
using RestSharp;
using RestSharp.Authenticators;
using Slithin.Marketplace.Models;
using SlithinMarketplace.Models;

namespace ApiConsole;

public class MarketplaceAPI
{
    private RestClient _client;
    private string _token;

    public MarketplaceAPI()
    {
        _client = new RestClient("https://slithin-api.multiplayer.co.at");
    }

    public void Authenticate(string username, string password)
    {
        var request = new RestRequest("/token", Method.Post)
            .AddBody(new Grant { grant_type = "password", username = username, password = password });

        var cts = new CancellationTokenSource();

        var result = _client.PostAsync<AuthenticationResult>(request, cts.Token);

        if (result != null)
        {
            _token = result.Result.access_token;
            _client.Authenticator = new JwtAuthenticator(_token);

            Console.WriteLine("Login Successful");
        }
    }

    public async void CreateAndUploadAsset(object? assetObj, string fileToUpload)
    {
        var request = new RestRequest($"/{assetObj.GetType().Name.ToLower()}s", Method.Put);
        request.AddBody(assetObj);

        var result = await _client.PutAsync<UploadRequest>(request);

        var wc = new WebClient();
        wc.Headers.Add("Authorization", "Bearer " + _token);

        var ms = new MemoryStream();
        using (var fs = File.OpenRead(fileToUpload))
        {
            await fs.CopyToAsync(ms);
        }

        wc.UploadData(_client.BuildUri(new RestRequest(result.UploadEndpoint)), ms.ToArray());
    }

    public void CreateAndUploadTemplate(Template? template, string templateInfoPath)
    {
        CreateAndUploadAsset(template, templateInfoPath);

        var templateBasePath = new FileInfo(templateInfoPath).Directory.FullName;
        var filePath = Path.Combine(templateBasePath, template.Filename);

        UploadFile(template.SvgFileID, filePath);
    }

    public T Get<T>(string bucket)
    {
        var request = new RestRequest($"/{bucket}", Method.Get);
        var r = _client.GetAsync(request).Result;

        return _client.GetAsync<T>(request).Result;
    }

    public async void UploadFile(string id, string fileToUpload)
    {
        var request = new RestRequest($"/files/request/{id}", Method.Get);

        var result = await _client.GetAsync<UploadRequest>(request);

        var wc = new WebClient();
        wc.Headers.Add("Authorization", "Bearer " + _token);

        var ms = new MemoryStream();
        using (var fs = File.OpenRead(fileToUpload))
        {
            await fs.CopyToAsync(ms);
        }

        wc.UploadData(_client.BuildUri(new RestRequest(result.UploadEndpoint)), ms.ToArray());
    }
}
