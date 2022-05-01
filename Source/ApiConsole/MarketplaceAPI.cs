using System.Net;
using ApiConsole.Core;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
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
        _client.UseNewtonsoftJson();
    }

    public async void Authenticate(string username, string password)
    {
        var request = new RestRequest("/token", Method.Post)
            .AddBody(new Grant { GrantType = "password", Username = username, Password = password });

        var cts = new CancellationTokenSource();

        var result = await _client.PostAsync<AuthenticationResult>(request, cts.Token);

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
        var templateBasePath = new FileInfo(templateInfoPath).Directory.FullName;

        var svgPath = Path.Combine(templateBasePath, template.Filename + ".svg");
        var pngPath = Path.Combine(templateBasePath, template.Filename + ".png");

        CreateAndUploadAsset(template, pngPath);

        UploadFile(template.SvgFileID, svgPath);
    }

    public T Get<T>(string bucket)
    {
        var request = new RestRequest($"/{bucket}", Method.Get);

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
