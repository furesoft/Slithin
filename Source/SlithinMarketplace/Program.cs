using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.BearerToken;
using SlithinMarketplace.Controller;
using Swan.Logging;

namespace SlithinMarketplace;

public static class Program
{
    public static async Task Main()
    {
        var url = "https://0.0.0.0:9696/";

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
            .WithWebApi("/", m =>
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
            .
            .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

        // Listen for state changes.
        server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

        return server;
    }
}
