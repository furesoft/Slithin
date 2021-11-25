namespace Slithin.Models;

public record ScriptInfo(string ID, string Name, string Category, string Description, bool IsAutomatable,
    bool IsListed)
{
    public object Config { get; set; }
}
