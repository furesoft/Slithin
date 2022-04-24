using System.Collections.Specialized;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace.Controller;

public sealed class ScreenController : WebApiController
{
    [Route(HttpVerbs.Get, "/screens/")]
    public object List([QueryData] NameValueCollection parameters)
    {
        HttpContext.RequireRole("Admin");

        return ServiceLocator.Repository.GetScreens().FilterByQuery(parameters);
    }
}
