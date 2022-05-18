using System.ComponentModel;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Slithin.Api.Swagger.Attributes;

namespace SlithinMarketplace.Controller;

public sealed class FilesController : WebApiController
{
    [Route(HttpVerbs.Get, "/files/{id}")]
    [Description("Get Content of file specified by id")]
    [ResponseContentType("application/octetstream")]
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
    [Description("Upload file specified by id")]
    public void Upload(string id)
    {
        var ms = new MemoryStream();
        HttpContext.Request.InputStream.CopyTo(ms);

        ServiceLocator.Repository.AddFile(id, ms);

        ms.Close();
    }

    [Route(HttpVerbs.Get, "/files/request/{id}")]
    [Description("Create upload request")]
    public object UploadRequest(string id)
    {
        return ServiceLocator.Repository.CreateUploadRequest(id);
    }
}
