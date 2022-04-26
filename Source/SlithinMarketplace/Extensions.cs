using System.Collections.Specialized;
using System.Security.Claims;
using EmbedIO;
using SlithinMarketplace.Models;

namespace SlithinMarketplace;

public static class Extensions
{
    public static IEnumerable<T> FilterByQuery<T>(this IEnumerable<T> collection, NameValueCollection parameters)
    {
        if (parameters["skip"] != null)
        {
            collection = collection.Skip(int.Parse(parameters["skip"]));
        }

        if (parameters["count"] != null)
        {
            collection = collection.Take(int.Parse(parameters["count"]));
        }

        return collection;
    }

    public static void InitAsset(this AssetModel asset, IHttpContext context)
    {
        asset.ID = Guid.NewGuid().ToString();
        asset.FileID = Guid.NewGuid().ToString();

        asset.CreatorID = ServiceLocator.Repository.GetUser(context.User.Identity.Name).ID;
    }

    public static void RequireAdmin(this IHttpContext context)
    {
        context.RequireRole("Admin");
    }

    public static void RequireRole(this IHttpContext context, string role)
    {
        var principal = (ClaimsPrincipal)context.User;
        var claims = principal.Claims;

        if (!principal.HasClaim(_ => _.Value == role && _.Type == "Role"))
        {
            throw new HttpException(401);
        }
    }
}
