using System.Collections.Generic;
using System.IO;
using Slithin.Core.Services;
using Slithin.ModuleSystem;
using WebAssembly.Runtime;

namespace Slithin.Core.Scripting
{
    public class Automation
    {
        public List<WebAssembly.Module> Modules = new();
        private static ImportDictionary _imports = new();
        private readonly IPathManager _pathManager;

        public Automation(IPathManager pathManager)
        {
            _pathManager = pathManager;
        }

        public ImportDictionary Imports { get => _imports; }

        //ToDo: Convert to WASM
        public void Evaluate(string scriptname)
        {
        }

        public void Init()
        {
            foreach (var m in Directory.GetFiles(_pathManager.ScriptsDir, "*.wasm"))
            {
                var module = ActionModule.LoadModule(m, out _imports);

                Modules.Add(module);
            }
        }
    }
}
