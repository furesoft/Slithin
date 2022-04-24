using EmbedIO;
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
}
