using Microsoft.OpenApi.Models;

namespace Slithin.Api.Swagger;

public class OpenAPI
{
    public static OpenApiDocument GetDocument()
    {
        var openApiSecurityRequirements = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "bearer",
                            Name = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                }
            };
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
            SecurityRequirements = openApiSecurityRequirements,
            Paths = new OpenApiPaths
            {
                ["/api/token"] = new OpenApiPathItem
                {
                    Operations = new Dictionary<OperationType, OpenApiOperation>
                    {
                        [OperationType.Post] = new OpenApiOperation
                        {
                            Description = "Returns a JWT Authentication Token",
                            Responses = new OpenApiResponses
                            {
                                ["200"] = new OpenApiResponse
                                {
                                    Description = "OK"
                                }
                            },
                        }
                    }
                },
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
                            },
                            Security = openApiSecurityRequirements
                        }
                    }
                }
            }
        };

        return document;
    }
}
