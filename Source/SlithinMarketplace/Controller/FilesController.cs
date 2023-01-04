using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace.Controller;

public sealed class FilesController : WebApiController
{
    [Route(HttpVerbs.Get, "/files/{id}")]
    public void GetFile(string id)
    {
        var fileStrm = ServiceLocator.Repository.GetFile("files", id);

        HttpContext.Response.ContentType = "application/octetstream";

        using (var stream = HttpContext.OpenResponseStream())
        {
            fileStrm.CopyTo(stream);
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

    [Route(HttpVerbs.Get, "/files/request/{id}")]
    public object UploadRequest(string id)
    {
        return ServiceLocator.Repository.CreateUploadRequest(id);
    }
}
