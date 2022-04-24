using System.Collections.Specialized;
using System.Security.Claims;
using EmbedIO;

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

    public static void RequireAdmin(this IHttpContext context)
    {
        context.RequireRole("Admin");
    }

    public static void RequireRole(this IHttpContext context, string role)
    {
        var principal = (ClaimsPrincipal)context.User;

        if (!principal.IsInRole(role))
        {
            throw new HttpException(401);
        }
    }
}
