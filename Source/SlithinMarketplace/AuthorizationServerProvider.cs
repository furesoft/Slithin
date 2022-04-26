using EmbedIO;
using EmbedIO.BearerToken;

namespace SlithinMarketplace;

internal class AuthorizationServerProvider : IAuthorizationServerProvider
{
    public long GetExpirationDate() => DateTime.UtcNow.AddHours(1).Ticks;

    public async Task ValidateClientAuthentication(ValidateClientAuthenticationContext context)
    {
        var data = await context.HttpContext.GetRequestDataAsync<Grant>();

        if (data != null && data.grant_type == "appid")
        {
            context.Identity.AddClaim(new System.Security.Claims.Claim("Role", "User"));

            if (data.appid != "SlithinBeta")
            {
                context.Rejected();
                context.Validated(string.Empty);
            }

            context.Validated(data.appid);
        }
        else
        {
            context.Rejected();
        }
    }

    private class Grant
    {
        public string appid { get; set; }
        public string grant_type { get; set; }
    }
}
