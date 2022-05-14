using EmbedIO.BearerToken;
using SlithinMarketplace.Models;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public long GetExpirationDate() => DateTime.UtcNow.AddHours(1).Ticks;

    public async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = await context.HttpContext.GetRequestObjectAsync<Grant>();

        if (data is null) Console.WriteLine("grant is null");

        if (data != null && data.GrantType == "password")
        {
            try
            {
                var user = ServiceLocator.Repository.GetUser(data.Username);
                context.Identity.AddClaim(new System.Security.Claims.Claim("Role", user?.Role == "admin" ? "Admin" : "User"));

                if (user == null || Utils.ComputeSha256Hash(data.Password) != user.HashedPassword)
                {
                    context.Rejected();
                }

                context.Validated(data.Username);
            }
            catch (Exception ex)
            {
                context.Rejected(ex);
            }
        }
        else
        {
            context.Rejected();
        }
    }
}
