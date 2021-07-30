using System;
using System.Threading.Tasks;
using NiL.JS;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using Slithin.Controls;

namespace Slithin.Core.Scripting
{
    public class ModuleResolver : CachedModuleResolverBase
    {
        public override bool TryGetModule(ModuleRequest moduleRequest, out Module result)
        {
            if (moduleRequest.CmdArgument.StartsWith("Slithin"))
            {
                result = new Module(moduleRequest.CmdArgument, "export { paths, openDialog };");
                result.Context.DefineVariable("ns").Assign(new NamespaceProvider(moduleRequest.CmdArgument));

                var paths = JSValue.Marshal(new { baseDir = ServiceLocator.ConfigBaseDir, templates = ServiceLocator.TemplatesDir, notebooks = ServiceLocator.NotebooksDir });

                result.Context.DefineVariable("paths").Assign(paths);
                result.Context.DefineVariable("openDialog").Assign(JSValue.Marshal(new Func<string, Task<bool>>(async (_) => await DialogService.ShowDialog(_))));

                return true;
            }

            result = null;
            return false;
        }
    }
}
