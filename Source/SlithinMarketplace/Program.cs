using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.BearerToken;
using SlithinMarketplace.Controller;
using Swan.Logging;

namespace SlithinMarketplace;

public static class Program
{
    public static void Main()
    {
        var url = "http://localhost:9696/";

        // Our web server is disposable.
        using (var server = CreateWebServer(url))
        {
            // Once we've registered our modules and configured them, we call the RunAsync() method.
            server.RunAsync();

#if DEBUG
            var browser = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true }
            };
            // browser.Start();
            // Wait for any key to be pressed before disposing of our web server.
            // In a service, we'd manage the lifecycle of our web server using
            // something like a BackgroundWorker or a ManualResetEvent.

#endif
            Console.ReadKey(true);
        }
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
            })

            .WithModule(new ActionModule("/", HttpVerbs.Any, ctx => ctx.SendDataAsync(new { Message = "Error" })));

        // Listen for state changes.
        server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

        return server;
    }
}
