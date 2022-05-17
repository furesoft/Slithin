using Microsoft.OpenApi.Models;

namespace Slithin.Api.Swagger;

public class OpenAPI
{
    public static OpenApiDocument GetDocument()
    {
        var document = new OpenApiDocument
        {
            Info = new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "Slithin Marketplace API",
            },
            Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = "https://slithin-api.multiplayer.co.at/" }
            },
            Paths = new OpenApiPaths
            {
                ["/pets"] = new OpenApiPathItem
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Get] = new OpenApiOperation
                        {
                            Description = "Returns all pets from the system that the user has access to",
                            Responses = new OpenApiResponses
                            {
                                ["200"] = new OpenApiResponse
                                {
                                    Description = "OK"
                                }
                            }
                        }
                    }
                }
            }
        };

        return document;
    }
}
