namespace Slithin.Modules.Menu.Models.ItemContext;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ContextAttribute : Attribute
{
    public ContextAttribute(string context)
    {
        Context = context;
    }

    public string Context { get; }
}
