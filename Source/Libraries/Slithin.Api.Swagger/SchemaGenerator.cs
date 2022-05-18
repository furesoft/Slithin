using System.Reflection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Slithin.Api.Swagger;

public static class SchemaGenerator
{
    public static OpenApiSchema GenerateSchema(this Type type)
    {
        var props = type.GetProperties();

        var properties = new Dictionary<string, OpenApiSchema>();

        foreach (var prop in props)
        {
            if (prop.PropertyType.Name == "ObjectId") continue;
            if (prop.PropertyType.Name == "DateTime") continue;

            string name = prop.Name.ToLower();

            var attribute = prop.GetCustomAttribute<JsonPropertyAttribute>();

            if (attribute is not null)
            {
                name = attribute.PropertyName;
            }

            properties.Add(name, new OpenApiSchema { Type = prop.PropertyType.Name.ToLower() });
        }

        return new OpenApiSchema
        {
            Type = "object",
            Properties = properties
        };
    }
}
