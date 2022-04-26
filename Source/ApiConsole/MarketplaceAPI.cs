using System.Net;
using ApiConsole.Core;
using RestSharp;
using RestSharp.Authenticators;
using SlithinMarketplace.Models;

namespace ApiConsole;

public class MarketplaceAPI
{
    private RestClient _client;
    private string _token;

    public MarketplaceAPI()
    {
        _client = new RestClient("http://slithin-api.multiplayer.co.at:9696");
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

    public T Get<T>(string bucket)
    {
        var request = new RestRequest($"/{bucket}", Method.Get);
        var r = _client.GetAsync(request).Result;

        return _client.GetAsync<T>(request).Result;
    }
}
