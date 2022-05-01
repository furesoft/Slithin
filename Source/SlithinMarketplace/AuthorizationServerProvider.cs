using EmbedIO.BearerToken;
using SlithinMarketplace.Models;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public long GetExpirationDate() => DateTime.UtcNow.AddHours(1).Ticks;

    public async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = await context.HttpContext.GetRequestObjectAsync<Grant>();

        if (data != null && data.grant_type == "password")
        {
            var user = ServiceLocator.Repository.GetUser(data.username);
            context.Identity.AddClaim(new System.Security.Claims.Claim("Role", user?.Role == "admin" ? "Admin" : "User"));

            if (user == null || Utils.ComputeSha256Hash(data.password) != user.HashedPassword)
            {
                context.Rejected();
                context.Validated(string.Empty);
            }

            context.Validated(data.username);
        }
        else
        {
            context.Rejected();
        }
    }
}
