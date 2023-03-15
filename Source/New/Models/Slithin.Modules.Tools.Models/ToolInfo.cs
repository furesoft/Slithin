using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Tools.Models;

public record ToolInfo(string ID, TranslatedString Name, TranslatedString Category, TranslatedString Description, bool IsAutomatable,
    bool IsListed)
{
    public object? Config { get; set; }
}
