using System;
using System.Threading.Tasks;
using NiL.JS;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using PdfSharpCore.Pdf;
using Slithin.Controls;
using Slithin.Core.Sync;

namespace Slithin.Core.Scripting
{
    public class ModuleResolver : CachedModuleResolverBase
    {
        public override bool TryGetModule(ModuleRequest moduleRequest, out Module result)
        {
            var mb = new ModuleBuilder();

            if (moduleRequest.CmdArgument == "slithin")
            {
                var paths = JSValue.Marshal(new { baseDir = ServiceLocator.ConfigBaseDir, templates = ServiceLocator.TemplatesDir, notebooks = ServiceLocator.NotebooksDir });

                mb.Add("paths", paths);
                mb.AddFunction("openDialog",
                               new Func<string, Task<bool>>(async (_) =>
                                    await DialogService.ShowDialog(_)));
                mb.AddFunction("showNotification",
                              new Action<string>((_) =>
                                  NotificationService.Show(_)));
            }
            else if (moduleRequest.CmdArgument == "slithin.sync")
            {
                mb.AddConstructor(typeof(SyncItem));
                mb.AddConstructor(typeof(SyncAction));
                mb.AddConstructor(typeof(SyncDirection));
                mb.AddConstructor(typeof(SyncType));
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
