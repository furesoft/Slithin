using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace.Controller;

internal class ScreenController : WebApiController
{
    [Route(HttpVerbs.Get, "/screens/list/{count?}/{skip?}")]
    public object List(int? count, int? skip)
    {
        return ServiceLocator.Repository.GetScreens(count, skip);
    }
}
