using System.Collections.Specialized;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using SlithinMarketplace.Models;

namespace SlithinMarketplace.Controller;

public sealed class ScreenController : WebApiController
{
    [Route(HttpVerbs.Get, "/screens/")]
    public object List([QueryData] NameValueCollection parameters)
    {
        return ServiceLocator.Repository.GetScreens().FilterByQuery(parameters);
    }

    [Route(HttpVerbs.Put, "/screens")]
    public async Task<UploadRequest> Upload()
    {
        var body = await HttpContext.GetRequestDataAsync<Screen>();
        body.InitAsset(HttpContext);

        ServiceLocator.Repository.AddScreen(body);

        return ServiceLocator.Repository.CreateUploadRequest(body.FileID);
    }
}
