using MessagePack;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WebAssembly;

namespace Slithin.ActionCompiler
{
    public class ModuleCompiler
    {
        public static Module Compile(string scriptFilename, string infoFilename, string uiFilename)
        {
            var m = new Module();

            //serialize scriptinfo into data segment
            var info = JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(infoFilename));
            var infoBytes = MessagePackSerializer.Serialize(info);

            m.CustomSections.Add(new CustomSection { Name = ".info", Content = new List<byte>(infoBytes) });
            m.Types.Add(new WebAssemblyType() { });

            m.Exports.Add(new Export("_start"));

            m.Functions.Add(new Function(0));
            m.Codes.Add(new FunctionBody(new WebAssembly.Instructions.End()));

            m.Imports.Add(new Import.Memory("env", "memory", new Memory(1, 25)));

            return m;
        }
    }
}

//ScriptInfo als data hinzufügen
//falls ui-xaml vorhanden, laden und als serialized in custom section speichern
//compilation des scripts mit start funktion in module