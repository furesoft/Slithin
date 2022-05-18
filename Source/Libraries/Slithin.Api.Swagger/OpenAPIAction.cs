using EmbedIO;
using EmbedIO.Actions;
using Microsoft.OpenApi.Writers;

namespace Slithin.Api.Swagger;

public static class OpenAPIAction
{
    public static WebServer WithOpenAPI(this WebServer opts, string baseRoute)
    {
        return opts.WithModule(new ActionModule(baseRoute, HttpVerbs.Get, (context) =>
        {
            context.Response.ContentType = "text/json";

            var server = opts;
            var document = OpenAPI.GetDocument(opts.Modules);

            var outputString = context.OpenResponseText();
            var writer = new OpenApiJsonWriter(outputString, new OpenApiJsonWriterSettings
            {
                Terse = true
            });

            document.SerializeAsV3(writer);

            outputString.Flush();

            return Task.CompletedTask;
        }));
    }
}
