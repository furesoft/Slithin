using EmbedIO;
using EmbedIO.BearerToken;
using EmbedIO.Utilities;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public long GetExpirationDate() => DateTime.UtcNow.AddHours(12).Ticks;

    public async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = await context.HttpContext.GetRequestFormDataAsync().ConfigureAwait(false);

        if (data?.ContainsKey("grant_type") == true && data["grant_type"] == "password")
        {
            context.Identity.AddClaim(new System.Security.Claims.Claim("Role", "User"));

            var hashedPassword = Utils.ComputeSha256Hash(data["password"]);

            context.Validated(data.ContainsKey("username") ? data["username"] : string.Empty);
        }
        else
        {
            context.Rejected();
        }
    }
}
