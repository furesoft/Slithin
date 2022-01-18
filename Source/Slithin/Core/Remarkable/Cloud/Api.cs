using System.IO;
using RestSharp;
using Slithin.Core.Services;

namespace Slithin.Core.Remarkable.Cloud;

public class Api
{
    public const string DeviceID = "9AB7CCA8-1B6D-4AAC-A07C-9831DF472D69";

    public string Authenticate(string otc)
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        var client = new RestClient("https://webapp-production-dot-remarkable-production.appspot.com/");

        var body = new
        {
            code = otc,
            deviceDesc = "desktop-windows",
            deviceID = DeviceID
        };

        var request = new RestRequest("token/json/2/device/new", Method.Post)
            .AddJsonBody(body)
            .AddHeader("Authorization", "Bearer ");

        var file = Path.Combine(pathManager.ConfigBaseDir, ".token");

        var response = client.PostAsync(request).Result;

        File.WriteAllText(file, response.Content);

        return response.Content;
    }

    public string GetStorageUrl()
    {
        var client = new RestClient("https://service-manager-production-dot-remarkable-production.appspot.com");

        var request = new RestRequest("/service/json/1/document-storage", Method.Get);
        request.AddQueryParameter("environment", "production");
        request.AddQueryParameter("group", "auth0|5a68dc51cb30df3877a1d7c4");
        request.AddQueryParameter("apiVer", "2");

        var response = client.GetAsync<GetStorageResult>(request).Result;

        return "https://" + response.Host;
    }

    public string GetToken(string otc = null)
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

        var file = Path.Combine(pathManager.ConfigBaseDir, ".token");
        if (!File.Exists(file))
        {
            File.WriteAllText(file, Authenticate(otc));
        }

        return File.ReadAllText(file);
    }

    public string RefreshToken()
    {
        var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
        var client = new RestClient("https://webapp-production-dot-remarkable-production.appspot.com/");

        var request = new RestRequest("/token/json/2/user/new", Method.Post);
        request.AddHeader("Authorization", "Bearer " + File.ReadAllText(Path.Combine(pathManager.ConfigBaseDir, ".token")));

        var response = client.PostAsync(request).Result;

        var content = response.Content;

        return content;
    }
}
