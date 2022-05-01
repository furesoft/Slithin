using EmbedIO.BearerToken;
using SlithinMarketplace.Models;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public long GetExpirationDate() => DateTime.UtcNow.AddHours(1).Ticks;

    public async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = await context.HttpContext.GetRequestObjectAsync<Grant>();

        if (data != null && data.GrantType == "password")
        {
            var user = ServiceLocator.Repository.GetUser(data.Username);
            context.Identity.AddClaim(new System.Security.Claims.Claim("Role", user?.Role == "admin" ? "Admin" : "User"));

            if (user == null || Utils.ComputeSha256Hash(data.Password) != user.HashedPassword)
            {
                context.Rejected();
                context.Validated(string.Empty);
            }

            context.Validated(data.Username);
        }
        else
        {
            context.Rejected();
        }
    }
}
