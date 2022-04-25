using System.Net.Http.Headers;
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
        _client = new RestClient("http://localhost:9696");
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

    public void CreateAndUploadAsset(object? assetObj, string fileToUpload)
    {
        var request = new RestRequest($"/{assetObj.GetType().Name.ToLower()}s", Method.Put);
        request.AddBody(assetObj);

        var result = _client.PutAsync<UploadRequest>(request).Result;

        var wc = new HttpClient();
        wc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        wc.BaseAddress = _client.BuildUri(new RestRequest(result.UploadEndpoint));

        var ms = new MemoryStream();
        using (var fs = File.OpenRead(fileToUpload))
        {
            fs.CopyTo(ms);
        }

        wc.PostAsync(result.UploadEndpoint, new StreamContent(ms));
    }

    public T Get<T>(string asset)
    {
        var request = new RestRequest($"/{asset}", Method.Get);
        var r = _client.GetAsync(request).Result;

        return _client.GetAsync<T>(request).Result;
    }
}
