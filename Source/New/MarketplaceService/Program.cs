using MarketplaceService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

namespace MarketplaceService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;

            var validator = new JwtTokenValidator();
            o.SecurityTokenValidators.Add(validator);
        });
        builder.Services.AddAuthorization();

        var app = builder.Build();
        IdentityModelEventSource.ShowPII = true;
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGrpcService<Authentication>();
        app.MapGrpcService<Storage>();

        app.MapGrpcReflectionService();

        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}
