using System.IO;
using Newtonsoft.Json;
using Slithin.Core;
using Slithin.Core.Scripting;
using Slithin.Core.Services;

namespace Slithin.Models
{
    public record ScriptInfo(string ID, string Name, string Category, string Description, bool isAutomatable)
    {
        public object Config { get; set; }

        public void Evaluate()
        {
            var automation = ServiceLocator.Container.Resolve<Automation>();

            automation.Evaluate(Name);
        }

        public void Save(object configObj)
        {
            Config = configObj;

            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            var content = JsonConvert.SerializeObject(this, Formatting.Indented);
            var file = Path.Combine(pathManager.ConfigBaseDir, "Scripts", Name + ".info");

            File.WriteAllText(file, content);
        }
    }
}
