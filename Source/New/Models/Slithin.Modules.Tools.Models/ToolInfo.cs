namespace Slithin.Modules.Tools.Models;

public record ToolInfo(string ID, string Name, string Category, string Description, bool IsAutomatable,
    bool IsListed)
{
    public object? Config { get; set; }
}
