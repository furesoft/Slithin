using EmbedIO;
using EmbedIO.BearerToken;
using EmbedIO.Utilities;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = await context.HttpContext.GetRequestFormDataAsync().ConfigureAwait(false);

        if (data?.ContainsKey("grant_type") == true && data["grant_type"] == "password")
        {
            context.Identity.AddClaim(new System.Security.Claims.Claim("Role", "Admin"));

            context.Validated(data.ContainsKey("username") ? data["username"] : string.Empty);
        }
        else
        {
            context.Rejected();
        }
    }

    public long GetExpirationDate() => DateTime.UtcNow.AddHours(12).Ticks;
}
