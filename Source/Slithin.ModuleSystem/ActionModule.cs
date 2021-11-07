using MessagePack;
using System.Linq;
using WebAssembly;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem
{
    public class ActionModule
    {
        public static dynamic Compile(Module m, ImportDictionary imports)
        {
            var compiled = m.Compile<dynamic>();
            var instance = compiled(imports);

            return instance.Exports;
        }

        public static ScriptInfo GetScriptInfo(Module module)
        {
            var section = module.CustomSections.First(_ => _.Name == ".info");

            return MessagePackSerializer.Deserialize<ScriptInfo>(section.Content.ToArray());
        }

        public static Module LoadModule(string path, out ImportDictionary imports)
        {
            var m = Module.ReadFromBinary(path);
            var wasi = new WASInterface.WASI(m);

            imports = wasi.CreateImports();

            return m;
        }

        public static void RunExports()
        {
            foreach (var type in ModuleImporter.Types)
            {
                ModuleImporter.Export(type);
            }
        }
    }
}