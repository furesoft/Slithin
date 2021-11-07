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
            //  m.Imports.Add(new Import.Memory("env", "memory"));

            //serialize scriptinfo into data segment
            var info = JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(infoFilename));
            var infoBytes = MessagePackSerializer.Serialize(info);

            m.CustomSections.Add(new CustomSection { Name = ".info", Content = new List<byte>(infoBytes) });

            return m;
        }
    }
}

//ScriptInfo als data hinzufügen
//falls ui-xaml vorhanden, laden und als serialized in custom section speichern
//compilation des scripts mit start funktion in module