using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace.Controller;

internal class FilesController : WebApiController
{
    [Route(HttpVerbs.Get, "/files/{bucket}/{id}")]
    public async Task GetFile(string bucket, string id)
    {
        var fileStrm = ServiceLocator.Repository.GetFile(bucket, id);

        using (var stream = HttpContext.OpenResponseStream())
        {
            await fileStrm.CopyToAsync(stream);
        }
    }
}
