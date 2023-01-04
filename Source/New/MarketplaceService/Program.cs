using System.Net;
using MarketplaceService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MarketplaceService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();
        builder.Services.AddGrpcReflection();

        builder.Services.AddSingleton<ProtoController>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;

            var validator = new JwtTokenValidator();
            o.SecurityTokenValidators.Add(validator);
        });
        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapGrpcService<Authentication>();
        app.MapGrpcService<Storage>();

        app.MapGrpcReflectionService();

        var protoService = app.Services.GetRequiredService<ProtoController>();

        app.MapGet("/protos/{protoName}", async context =>
        {
            var protoName = (string)context.Request.RouteValues["protoName"];

            var content = protoService.Get(protoName);

            if (content != null)
            {
                await content.CopyToAsync(context.Response.Body);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        });

        app.MapGet("/", async context =>
        {
            var filenames = protoService.GetAvailable();

            var links = string.Join('\n',
                            filenames.Select(_ => $"<a href=http://{context.Request.Host + "/protos/" + _}>{Path.GetFileNameWithoutExtension(_)}<a/>"));

            await context.Response.WriteAsync($"<html><head><title>Services</title></head><body>{links}</body></html>");
        });

        app.Run();
    }
}
