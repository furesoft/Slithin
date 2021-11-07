using System.Collections.Generic;
using System.IO;
using System.Linq;
using NiL.JS;
using NiL.JS.Core;
using NiL.JS.Core.Functions;
using NiL.JS.Extensions;
using Slithin.Core.Services;
using Slithin.ModuleSystem;

namespace Slithin.Core.Scripting
{
    public class Automation
    {
        public List<WebAssembly.Module> Modules = new();
        private readonly IPathManager _pathManager;

        public Automation(IPathManager pathManager)
        {
            _pathManager = pathManager;
        }

        //ToDo: Convert to WASM
        public void Evaluate(string scriptname)
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            var path = Path.Combine(pathManager.ScriptsDir, scriptname + ".js");

            if (File.Exists(path))
            {
                var mainModule = new Module($"Scripts/{scriptname}.js", File.ReadAllText(path));
                mainModule.Context.DefineVariable("events").Assign(JSValue.Wrap(ServiceLocator.Container.Resolve<EventStorage>()));
                //mainModule.Context.DefineVariable("config", false).Assign(Utils.ToJSObject((JObject)GetScriptInfo(scriptname).Config));
                mainModule.Context.DefineVariable("saveConfig").Assign(new ExternalFunction((t, args) =>
                {
                    var confObj = Utils.ConvertToJObject(args.First().Value.As<JSObject>());

                    // GetScriptInfo(scriptname).Save(confObj);
                    return null;
                }));

                mainModule.ModuleResolversChain.Add(ServiceLocator.Container.Resolve<ModuleResolver>());

                mainModule.Run();
            }
        }

        public void Init()
        {
            foreach (var m in Directory.GetFiles(_pathManager.ScriptsDir, "*.wasm"))
            {
                var module = ActionModule.LoadModule(m, out var imports);

                Modules.Add(module);
            }
        }
    }
}
