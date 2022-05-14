using EmbedIO.BearerToken;
using SlithinMarketplace.Models;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public long GetExpirationDate() => DateTime.UtcNow.AddHours(1).Ticks;

    public Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = context.HttpContext.GetRequestObjectAsync<Grant>().Result;
        
        Console.WriteLine(data.Username);

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

        return Task.CompletedTask;
    }
}
