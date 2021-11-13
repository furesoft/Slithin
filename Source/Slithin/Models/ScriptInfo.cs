using Slithin.Core;
using Slithin.Core.Scripting;

namespace Slithin.Models
{
    public record ScriptInfo(string ID, string Name, string Category, string Description, bool isAutomatable)
    {
        public object Config { get; set; }
    }
}
