namespace Slithin.Api.Swagger.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class BodyTypeAttribute : Attribute
{
    public BodyTypeAttribute(Type type)
    {
        Type = type;
    }

    public Type Type { get; set; }
}
