using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Grpc.Core;
using MarketplaceService.Protos;
using Microsoft.IdentityModel.Tokens;

namespace MarketplaceService.Services;

public class Authentication : AuthenticationService.AuthenticationServiceBase
{
    public override async Task<AuthResult> Authenticate(Grant request, ServerCallContext context)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Role, "admin"));
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, request.Username));
        claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, Guid.NewGuid().ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my super secret key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: "Furesoft", expires: DateTime.Today.AddDays(300),
            claims: claims, signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler();
        var encodedToken = handler.WriteToken(token);

        return new AuthResult { Token = encodedToken };
    }
}
