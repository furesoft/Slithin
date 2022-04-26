﻿using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace.Controller;

public sealed class FilesController : WebApiController
{
    [Route(HttpVerbs.Get, "/files/{id}")]
    public async Task GetFile(string id)
    {
        var fileStrm = ServiceLocator.Repository.GetFile("files", id);

        HttpContext.Response.ContentType = "application/octetstream";

        using (var stream = HttpContext.OpenResponseStream())
        {
            await fileStrm.CopyToAsync(stream);
        }
    }

    [Route(HttpVerbs.Post, "/files/upload/{id}")]
    public void Upload(string id)
    {
        var ms = new MemoryStream();
        HttpContext.Request.InputStream.CopyTo(ms);

        ServiceLocator.Repository.AddFile(id, ms);

        ms.Close();
    }
}
