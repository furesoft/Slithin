using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NiL.JS;
using NiL.JS.Core;
using NiL.JS.Core.Functions;
using NiL.JS.Extensions;
using Slithin.Core.Scripting.Extensions;
using Slithin.Core.Services;

namespace Slithin.Core.Scripting
{
    public static class Automation
    {
        public static void Evaluate(string scriptname)
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            var path = Path.Combine(pathManager.ScriptsDir, scriptname + ".js");

            if (File.Exists(path))
            {
                var mainModule = new Module($"Scripts/{scriptname}.js", File.ReadAllText(path));
                mainModule.Context.DefineVariable("events").Assign(JSValue.Wrap(ServiceLocator.Container.Resolve<EventStorage>()));
                mainModule.Context.DefineVariable("config", false).Assign(Utils.ToJSObject((JObject)GetScriptInfo(scriptname).Config));
                mainModule.Context.DefineVariable("saveConfig").Assign(new ExternalFunction((t, args) =>
                {
                    var confObj = Utils.ConvertToJObject(args.First().Value.As<JSObject>());

                    GetScriptInfo(scriptname).Save(confObj);
                    return null;
                }));

                mainModule.ModuleResolversChain.Add(ServiceLocator.Container.Resolve<ModuleResolver>());

                mainModule.Run();
            }
        }

        public static ScriptInfo GetScriptInfo(string scriptName)
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            var file = Path.Combine(pathManager.ConfigBaseDir, "Scripts", scriptName + ".info");
            return JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(file));
        }

        public static IEnumerable<ScriptInfo> GetScriptInfos()
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            foreach (var file in Directory.GetFiles(Path.Combine(pathManager.ConfigBaseDir, "Scripts"), "*.info"))
            {
                yield return JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(file));
            }
        }

        public static string[] GetScriptNames()
        {
            var pathManager = ServiceLocator.Container.Resolve<IPathManager>();

            return Directory.GetFiles(
                    Path.Combine(pathManager.ConfigBaseDir, "Scripts")).
                    Select(_ => Path.GetFileNameWithoutExtension(_)).
                    ToArray();
        }

        public static void Init()
        {
            Parser.DefineCustomCodeFragment(typeof(UsingStatement));
            Parser.DefineCustomCodeFragment(typeof(OnCallStatement));

            Parser.DefineCustomCodeFragment(typeof(KeysOfOperator));
        }
    }
}
