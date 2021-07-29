using NiL.JS.Core;

namespace Slithin.Core
{
    public record ScriptInfo(string Name, string Category, string Description)
    {
        public void Evaluate()
        {
            Automation.Evaluate(Name);
        }
    }
}
