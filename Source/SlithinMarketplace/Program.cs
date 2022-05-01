using System.Text;
using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.BearerToken;
using EmbedIO.Utilities;
using Newtonsoft.Json;
using SlithinMarketplace.Controller;
using Swan.Logging;

namespace SlithinMarketplace;

public static class Program
{
    public static async Task Main()
    {
        var url = "http://*:9696/";

        // Our web server is disposable.
        var server = CreateWebServer(url);

        // Once we've registered our modules and configured them, we call the RunAsync() method.
        await server.RunAsync();
    }

    private static WebServer CreateWebServer(string url)
    {
        var basicAuthProvider = new AuthorizationServerProvider();

        var server = new WebServer(o => o
                .WithUrlPrefix(url)
                .WithMode(HttpListenerMode.EmbedIO))
            // First, we will configure our web server by adding Modules.
            .WithModule(new BearerTokenModule("/", basicAuthProvider, new string('f', 40)))
            .WithWebApi("/", SerializeCalback, m =>
             {
                 m.RegisterController<ScreenController>();
                 m.RegisterController<FilesController>();
                 m.RegisterController<TemplatesController>();
             })
            .HandleHttpException((context, ex) =>
            {
                context.SendDataAsync(new { ex.StatusCode });

                return Task.CompletedTask;
            })

            .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

        // Listen for state changes.
        server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

        return server;
    }

    private static async Task SerializeCalback(IHttpContext context, object? data)
    {
        Validate.NotNull(nameof(context), context).Response.ContentType = MimeType.Json;
        using var text = context.OpenResponseText(new UTF8Encoding(false));

        var serializerSettings = new JsonSerializerSettings();
        serializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;

        var json = JsonConvert.SerializeObject(data, Formatting.Indented, serializerSettings);

        await text.WriteAsync(json).ConfigureAwait(false);
    }
}
