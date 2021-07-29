using NiL.JS;
using NiL.JS.Core;

namespace Slithin.Core.Scripting
{
    public class ModuleResolver : CachedModuleResolverBase
    {
        public override bool TryGetModule(ModuleRequest moduleRequest, out Module result)
        {
            if (moduleRequest.CmdArgument.StartsWith("Slithin"))
            {
                result = new Module(moduleRequest.CmdArgument, "export { paths };");
                result.Context.DefineVariable("ns").Assign(new NamespaceProvider(moduleRequest.CmdArgument));

                var paths = JSValue.Marshal(new { baseDir = ServiceLocator.ConfigBaseDir, templates = ServiceLocator.TemplatesDir, notebooks = ServiceLocator.NotebooksDir });

                result.Context.DefineVariable("paths").Assign(paths);

                return true;
            }

            result = null;
            return false;
        }
    }
}
