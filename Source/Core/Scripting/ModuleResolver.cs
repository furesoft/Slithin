using System;
using System.Threading.Tasks;
using NiL.JS;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using PdfSharpCore.Pdf;
using Slithin.Controls;

namespace Slithin.Core.Scripting
{
    public class ModuleResolver : CachedModuleResolverBase
    {
        public override bool TryGetModule(ModuleRequest moduleRequest, out Module result)
        {
            var mb = new ModuleBuilder();

            if (moduleRequest.CmdArgument.StartsWith("Slithin"))
            {
                var paths = JSValue.Marshal(new { baseDir = ServiceLocator.ConfigBaseDir, templates = ServiceLocator.TemplatesDir, notebooks = ServiceLocator.NotebooksDir });

                mb.Add("paths", paths);
                mb.Add("openDialog", JSValue.Marshal(new Func<string, Task<bool>>(async (_) => await DialogService.ShowDialog(_))));
            }
            else if (moduleRequest.CmdArgument == "pdf")
            {
                mb.Add(new NamespaceProvider(typeof(PdfDocument).Namespace));
            }
            else
            {
                result = null;
                return false;
            }

            result = mb.Build(moduleRequest.CmdArgument);
            return true;
        }
    }
}
