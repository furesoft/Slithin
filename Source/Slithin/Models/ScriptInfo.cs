namespace Slithin.Models;

public record ScriptInfo(string ID, string Name, string Category, string Description, bool IsAutomatable,
    bool IsListed, bool isVPL)
{
    public object Config { get; set; }
}
