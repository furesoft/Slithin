using System.IO;
using Newtonsoft.Json;
using Slithin.Core.Scripting;
using Slithin.Core.Services;
using Slithin.Core;

namespace Slithin.Models
{
    public record ScriptInfo(string Name, string Category, string Description)
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
