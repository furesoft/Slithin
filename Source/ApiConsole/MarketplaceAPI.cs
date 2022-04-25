﻿using ApiConsole.Core;
using RestSharp;
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

            Console.WriteLine("Login Successful");
        }
    }

    public T Get<T>(string asset)
    {
        var request = new RestRequest($"/{asset}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {_token}");
        var r = _client.GetAsync(request).Result;

        return _client.GetAsync<T>(request).Result;
    }

    internal UploadRequest CreateScreen(Screen? screen)
    {
        throw new NotImplementedException();
    }
}
