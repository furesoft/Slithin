using System.ComponentModel;
using System.Reflection;
using EmbedIO;
using EmbedIO.BearerToken;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Microsoft.OpenApi.Models;
using Slithin.Api.Swagger.Attributes;

namespace Slithin.Api.Swagger;

public class OpenAPI
{
    public static OpenApiDocument GetDocument(IEnumerable<IWebModule> modules)
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

        var paths = new OpenApiPaths();

        foreach (var module in modules)
        {
            if (module is BearerTokenModule bearerTokenModule)
            {
                paths.Add("/token", GetTokenPath(bearerTokenModule));
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
            Paths = paths
        };

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
                var attribute = op.GetCustomAttribute<RouteAttribute>();

                if (attribute is not null)
                {
                    var o = GetOperation(attribute, op);

                    if (!path.Operations.ContainsKey(o.Item1))
                    {
                        path.Operations.Add(o.Item1, o.Item2);
                    }

                    yield return (attribute.Route, path);
                }
            }
        }

        static (OperationType, OpenApiOperation) GetOperation(RouteAttribute route, MethodInfo methodInfo)
        {
            return (GetOperationType(route.Verb), new OpenApiOperation
            {
                Description = GetDescription(methodInfo),
                Parameters = GetParameters(methodInfo),
                Tags = new List<OpenApiTag>() { GetTag(methodInfo) },
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
                                Type = "grant"
                            }
                        }
                    }
                }
            });
        }
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
                                            Type = "grant"
                                        }
                                    }
                                }
                    },
                }
            }
        };
    }
}
