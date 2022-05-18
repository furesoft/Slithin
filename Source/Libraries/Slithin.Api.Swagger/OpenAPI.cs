using System.ComponentModel;
using System.Reflection;
using EmbedIO;
using EmbedIO.BearerToken;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Microsoft.OpenApi.Models;
using Slithin.Api.Swagger.Attributes;
using SlithinMarketplace.Models;

namespace Slithin.Api.Swagger;

public class OpenAPI
{
    private static readonly Dictionary<string, OpenApiSchema> Schemas = new();

    public static void AddSchema(Type type)
    {
        if (!Schemas.ContainsKey(type.Name))
        {
            Schemas.Add(type.Name, type.GenerateSchema());
        }
    }

    public static OpenApiDocument GetDocument(IEnumerable<IWebModule> modules)
    {
        var openApiSecurityRequirements = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Scheme = "bearer",
                            Name = "Bearer",
                            Type = SecuritySchemeType.Http,
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                }
            };

        var paths = new OpenApiPaths();

        foreach (var module in modules)
        {
            if (module is BearerTokenModule bearerTokenModule)
            {
                paths.Add("/token", GetTokenPath(bearerTokenModule));
                AddSchema(typeof(Grant));
            }
            else if (module is WebApiModule webApiModule)
            {
                foreach (var item in GetSanitisedPath(webApiModule))
                {
                    var endpoint = webApiModule.BaseRoute + item.Item1;
                    paths.Add(endpoint.Replace("//", "/"), item.Item2);
                }
            }
        }

        var document = new OpenApiDocument
        {
            Info = new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "Slithin Marketplace API",
                Description = "This API is used to save/restore assets",
            },
            Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = "https://slithin-api.multiplayer.co.at/" }
            },
            SecurityRequirements = openApiSecurityRequirements,
            Paths = paths,
            Components = new OpenApiComponents() { Schemas = Schemas }
        };

        document.ResolveReferences();

        return document;
    }

    private static string GetDescription(MethodInfo methodInfo)
    {
        var attribute = methodInfo.GetCustomAttribute<DescriptionAttribute>();

        return attribute is not null ? attribute.Description : "";
    }

    private static string GetDescription(Type controller)
    {
        var attribute = controller.GetCustomAttribute<DescriptionAttribute>();

        return attribute is not null ? attribute.Description : "";
    }

    private static (OperationType, OpenApiOperation) GetOperation(RouteAttribute route, MethodInfo methodInfo)
    {
        var bodyType = methodInfo.GetCustomAttribute<BodyTypeAttribute>();
        var responseType = methodInfo.GetCustomAttribute<ResponseContentTypeAttribute>();

        if (bodyType is not null)
        {
            AddSchema(bodyType.Type);
        }

        return (GetOperationType(route.Verb), new OpenApiOperation
        {
            Description = GetDescription(methodInfo),
            Parameters = GetParameters(methodInfo),
            Tags = new List<OpenApiTag>() { GetTag(methodInfo) },
            Security = new List<OpenApiSecurityRequirement>()
            {
               new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        }
                    ] = new List<string>()
                   }
            },
            Responses = new OpenApiResponses
            {
                ["200"] = GetResponse(methodInfo, responseType),
                ["401"] = new OpenApiResponse()
                {
                    Description = "Unauthorized"
                },
            },
            RequestBody = new OpenApiRequestBody
            {
                Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                            {
                                Schema = bodyType is not null ? new OpenApiSchema
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Id = bodyType.Type.Name,
                                            Type = ReferenceType.Schema
                                    }
                                } : null
                        }
                    }
            }
        });
    }

    private static OperationType GetOperationType(HttpVerbs verb)
    {
        return verb switch
        {
            HttpVerbs.Delete => OperationType.Delete,
            HttpVerbs.Get => OperationType.Get,
            HttpVerbs.Head => OperationType.Head,
            HttpVerbs.Options => OperationType.Options,
            HttpVerbs.Patch => OperationType.Patch,
            HttpVerbs.Post => OperationType.Post,
            HttpVerbs.Put => OperationType.Put,
            _ => OperationType.Get,
        };
    }

    private static IList<OpenApiParameter> GetParameters(MethodInfo methodInfo)
    {
        var result = new List<OpenApiParameter>();

        foreach (var parameter in methodInfo.GetParameters())
        {
            var queryData = parameter.GetCustomAttribute<QueryDataAttribute>();

            if (queryData is not null)
            {
                result.Add(new()
                {
                    Name = "skip",
                    In = ParameterLocation.Query,
                });
                result.Add(new()
                {
                    Name = "count",
                    In = ParameterLocation.Query,
                });
            }
            else
            {
                result.Add(new()
                {
                    Name = parameter.Name,
                    In = ParameterLocation.Path,
                });
            }
        }

        return result;
    }

    private static IEnumerable<(string, OpenApiPathItem)> GetPaths(WebApiModule webApiModule)
    {
        var controllerTypes = webApiModule.GetType().BaseType.GetField("_controllerTypes", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
        foreach (var controller in (HashSet<Type>)controllerTypes.GetValue(webApiModule))
        {
            var operations = controller.GetMethods().Where(_ => _.CustomAttributes.Any());

            var path = new OpenApiPathItem();
            path.Operations = new Dictionary<OperationType, OpenApiOperation>();
            path.Description = GetDescription(controller);

            foreach (var op in operations)
            {
                var route = op.GetCustomAttribute<RouteAttribute>();

                if (route is not null)
                {
                    var o = GetOperation(route, op);

                    if (!path.Operations.ContainsKey(o.Item1))
                    {
                        path.Operations.Add(o.Item1, o.Item2);
                    }

                    yield return (route.Route, path);
                }
            }
        }
    }

    private static OpenApiResponse GetResponse(MethodInfo methodInfo, ResponseContentTypeAttribute? responseType)
    {
        return new OpenApiResponse
        {
            Description = "OK",
            Content = new Dictionary<string, OpenApiMediaType>
            {
                [responseType is not null ? responseType.Type : "application/json"] = new OpenApiMediaType
                {
                    Schema = responseType is not null ? new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = methodInfo.ReturnType.Name
                        }
                    } : new OpenApiSchema
                    {
                        Format = "binary"
                    }
                }
            }
        };
    }

    private static IEnumerable<(string, OpenApiPathItem)> GetSanitisedPath(WebApiModule webApiModule)
    {
        var paths = GetPaths(webApiModule).ToArray();
        var result = new Dictionary<string, OpenApiPathItem>();

        foreach (var path in paths)
        {
            var endpoint = path.Item1.Replace("//", "/");

            if (result.ContainsKey(endpoint))
            {
                result[endpoint] = path.Item2;
            }
            else
            {
                result.Add(endpoint, path.Item2);
            }
        }

        return result.Select(_ => (_.Key, _.Value));
    }

    private static OpenApiTag GetTag(MethodInfo methodInfo)
    {
        var attr = methodInfo.GetCustomAttribute<WithoutAuthenticationAttribute>();

        return attr is null ? new OpenApiTag { Name = "Authorized" } : new OpenApiTag { Name = "Default" };
    }

    private static OpenApiPathItem GetTokenPath(BearerTokenModule bearerTokenModule)
    {
        return new OpenApiPathItem
        {
            Operations = new Dictionary<OperationType, OpenApiOperation>
            {
                [OperationType.Post] = new OpenApiOperation
                {
                    Tags = new List<OpenApiTag>() { new() { Name = "Default" } },
                    Description = "Returns a JWT Authentication Token",
                    Responses = new OpenApiResponses
                    {
                        ["200"] = new OpenApiResponse
                        {
                            Description = "OK"
                        },
                        ["401"] = new OpenApiResponse()
                        {
                            Description = "Unauthorized"
                        }
                    },
                    RequestBody = new OpenApiRequestBody
                    {
                        Content =
                                {
                                    ["application/json"] = new OpenApiMediaType
                                    {
                                        Schema = new OpenApiSchema
                                        {
                                            Reference = new OpenApiReference
                                            {
                                                Id = "Grant",
                                                Type = ReferenceType.Schema
                                            }
                                        }
                                    }
                                }
                    },
                }
            }
        };
    }
}
