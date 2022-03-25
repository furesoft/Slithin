using EmbedIO;
using EmbedIO.BearerToken;
using EmbedIO.Utilities;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public long GetExpirationDate() => DateTime.UtcNow.AddHours(1).Ticks;

    public async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = await context.HttpContext.GetRequestFormDataAsync().ConfigureAwait(false);

        if (data?.ContainsKey("grant_type") == true && data["grant_type"] == "appid")
        {
            context.Identity.AddClaim(new System.Security.Claims.Claim("Role", "User"));

            if (data["appid"] != "SlithinBetaID")
            {
                context.Rejected();
            }

            context.Validated(data.ContainsKey("username") ? data["username"] : string.Empty);
        }
        else
        {
            context.Rejected();
        }
    }
}
