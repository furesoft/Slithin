namespace Slithin.Api.Swagger.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class ResponseContentTypeAttribute : Attribute
{
    public ResponseContentTypeAttribute(string type)
    {
        Type = type;
    }

    public string Type { get; set; }
}
