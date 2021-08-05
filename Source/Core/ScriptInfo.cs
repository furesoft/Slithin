using System.IO;
using Newtonsoft.Json;
using Slithin.Core.Scripting;

namespace Slithin.Core
{
    public record ScriptInfo(string Name, string Category, string Description)
    {
        public object Config { get; set; }

        public void Evaluate()
        {
            Automation.Evaluate(Name);
        }

        public void Save(object configObj)
        {
            Config = configObj;

            var content = JsonConvert.SerializeObject(this, Formatting.Indented);
            var file = Path.Combine(ServiceLocator.ConfigBaseDir, "Scripts", Name + ".info");

            File.WriteAllText(file, content);
        }
    }
}
