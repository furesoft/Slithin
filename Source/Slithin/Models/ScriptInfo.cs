namespace Slithin.Models;

public record ScriptInfo(string ID, string Name, string Category, string Description, bool IsAutomatable)
{
    public object Config { get; set; }
}
