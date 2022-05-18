using System.Collections.Specialized;
using System.ComponentModel;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Slithin.Api.Swagger.Attributes;
using SlithinMarketplace.Models;

namespace SlithinMarketplace.Controller;

public sealed class ScreenController : WebApiController
{
    [Route(HttpVerbs.Get, "/screens")]
    [Description("List screens")]
    public object List([QueryData] NameValueCollection parameters)
    {
        return ServiceLocator.Repository.GetAssets<Screen>("screens").FilterByQuery(parameters);
    }

    [Route(HttpVerbs.Put, "/screens")]
    [Description("Add a new screen")]
    [BodyType(typeof(Screen))]
    public async Task<UploadRequest> Upload()
    {
        var body = await HttpContext.GetRequestObjectAsync<Screen>();
        body.InitAsset(HttpContext);

        ServiceLocator.Repository.AddAsset("screens", body);

        return ServiceLocator.Repository.CreateUploadRequest(body.FileID);
    }
}
