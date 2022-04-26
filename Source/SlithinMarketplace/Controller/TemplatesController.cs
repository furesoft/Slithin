using System.Collections.Specialized;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Slithin.Marketplace.Models;
using SlithinMarketplace.Models;

namespace SlithinMarketplace.Controller;

public sealed class TemplatesController : WebApiController
{
    [Route(HttpVerbs.Get, "/templates")]
    public object List([QueryData] NameValueCollection parameters)
    {
        return ServiceLocator.Repository.GetTemplates().FilterByQuery(parameters);
    }

    [Route(HttpVerbs.Put, "/templates")]
    public async Task<UploadRequest> Upload()
    {
        var body = await HttpContext.GetRequestDataAsync<Template>();
        body.InitAsset(HttpContext);

        ServiceLocator.Repository.AddTemplate(body);

        return ServiceLocator.Repository.CreateUploadRequest(body.FileID);
    }
}
