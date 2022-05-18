using System.Collections.Specialized;
using System.ComponentModel;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Slithin.Marketplace.Models;
using SlithinMarketplace.Models;

namespace SlithinMarketplace.Controller;

public sealed class TemplatesController : WebApiController
{
    [Route(HttpVerbs.Get, "/templates")]
    [Description("List templates")]
    public object List([QueryData] NameValueCollection parameters)
    {
        return ServiceLocator.Repository.GetAssets<Template>("templates").FilterByQuery(parameters);
    }

    [Route(HttpVerbs.Put, "/templates")]
    [Description("Add a template")]
    public async Task<UploadRequest> Upload()
    {
        var body = await HttpContext.GetRequestObjectAsync<Template>();
        body.InitAsset(HttpContext);

        ServiceLocator.Repository.AddAsset("templates", body);

        return ServiceLocator.Repository.CreateUploadRequest(body.FileID);
    }
}
