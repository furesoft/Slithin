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
        var token = CreateToken(request.Username, "admin");

        return new AuthResult { Token = token };
    }

    public override async Task<AuthResult> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
    {
        var handler = new JwtSecurityTokenHandler();

        var oldToken = handler.ReadJwtToken(request.Token);

        var newToken = CreateToken(oldToken.Subject, oldToken.Claims.First(_ => _.Type == "roles").Value);

        return new AuthResult { Token = newToken };
    }

    private static string CreateToken(string username, string role)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim("roles", role));
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, username));
        claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my super secret key"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: "Furesoft", expires: DateTime.Today.AddDays(300),
            claims: claims, signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler();

        return handler.WriteToken(token);
    }
}
