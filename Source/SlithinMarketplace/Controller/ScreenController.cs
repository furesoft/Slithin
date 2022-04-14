using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace SlithinMarketplace.Controller;

internal class ScreenController : WebApiController
{
    [Route(HttpVerbs.Get, "/screens/list/{count}")]
    public string ListScreens(int count)
    {
        return ServiceLocator.Repository.GetScreens(count);
    }
}
