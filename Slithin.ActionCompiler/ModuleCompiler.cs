using Newtonsoft.Json;
using System.IO;
using WebAssembly;

namespace Slithin.ActionCompiler
{
    public struct ScriptInfo
    {
        //
    }

    public class ModuleCompiler
    {
        public static Module Compile(string scriptFilename, string infoFilename, string uiFilename)
        {
            var m = new Module();
            m.Imports.Add(new Import.Memory("env", "memory"));

            //serialize scriptinfo into data segment
            var info = JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(infoFilename));
            var infoBytes = Utils.GetBytes(info);

            m.Data.Add(new Data() { Index = 0, RawData = infoBytes, InitializerExpression = new[] { new WebAssembly.Instructions.Int32Constant() { Value = 1 } } });

            return m;
        }
    }
}

//ScriptInfo als data hinzufügen
//falls ui-xaml vorhanden, laden und als serialized in custom section speichern
//compilation des scripts mit start funktion in module