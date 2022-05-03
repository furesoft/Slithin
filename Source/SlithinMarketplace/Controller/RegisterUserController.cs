using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using SlithinMarketplace.Models;

namespace SlithinMarketplace.Controller;

public sealed class RegisterUserController : WebApiController
{
    [Route(HttpVerbs.Put, "/register")]
    public void Register()
    {
        var user = HttpContext.GetRequestObjectAsync<User>().Result;

        ServiceLocator.Repository.AddUser(user.Username, user.HashedPassword);
    }
}
