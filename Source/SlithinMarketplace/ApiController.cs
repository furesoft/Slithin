using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace;

internal class ApiController : WebApiController
{
    [Route(HttpVerbs.Get, "/hello/{name}")]
    public string SayHello(string name)
    {
        return $"Hello, {name}";
    }
}
