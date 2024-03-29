﻿using System.Collections.Specialized;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace.Controller;

public sealed class ScreenController : WebApiController
{
    [Route(HttpVerbs.Get, "/screens")]
    public object List([QueryData] NameValueCollection parameters)
    {
        return ServiceLocator.Repository.GetAssets<Screen>("screens").FilterByQuery(parameters);
    }

    [Route(HttpVerbs.Put, "/screens")]
    public async Task<UploadRequest> Upload()
    {
        var body = await HttpContext.GetRequestObjectAsync<Screen>();
        body.InitAsset(HttpContext);

        ServiceLocator.Repository.AddAsset("screens", body);

        return ServiceLocator.Repository.CreateUploadRequest(body.FileID);
    }
}
