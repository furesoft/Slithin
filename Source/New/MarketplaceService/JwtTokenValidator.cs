using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MarketplaceService;

public class JwtTokenValidator : ISecurityTokenValidator
{
    public bool CanValidateToken { get; } = true;

    public int MaximumTokenSizeInBytes { get; set; } = int.MaxValue;

    public bool CanReadToken(string securityToken) => true;

    public ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Furesoft",
            RoleClaimType = "role",
            ValidAudience = "Slithin",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my super secret key"))
        };

        var claimsPrincipal = handler.ValidateToken(token, tokenValidationParameters, out validatedToken);

        return claimsPrincipal;
    }
}
