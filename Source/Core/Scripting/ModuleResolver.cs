using System;
using System.IO;
using System.Threading.Tasks;
using NiL.JS;
using NiL.JS.Core;
using PdfSharpCore.Pdf;
using Slithin.Controls;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.Core.Scripting
{
    public class ModuleResolver : CachedModuleResolverBase
    {
        private readonly IPathManager _pathManager;

        public ModuleResolver(IPathManager pathManager)
        {
            _pathManager = pathManager;
        }

        public override bool TryGetModule(ModuleRequest moduleRequest, out Module result)
        {
            var mb = new ModuleBuilder();

            if (moduleRequest.CmdArgument == "slithin")
            {
                var paths = JSValue.Marshal(new { baseDir = _pathManager.ConfigBaseDir, templates = _pathManager.TemplatesDir, notebooks = _pathManager.NotebooksDir });

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

                mb.AddFunction("StartSync",
                              new Action(() =>
                                  ServiceLocator.SyncService.SynchronizeCommand.Execute(null)));
            }
            else if (moduleRequest.CmdArgument == "slithin.mailbox")
            {
                var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

                mb.AddFunction("post",
                             new Action<AsynchronousMessage>(mailboxService.Post));

                mb.AddConstructor(typeof(Messages.AttentionRequiredMessage));
                mb.AddConstructor(typeof(Messages.DownloadNotebooksMessage));
                mb.AddConstructor(typeof(Messages.HideStatusMessage));
                mb.AddConstructor(typeof(Messages.InitStorageMessage));
                mb.AddConstructor(typeof(Messages.ShowStatusMessage));
            }
            else if (moduleRequest.CmdArgument == "pdf")
            {
                mb.Add(new NamespaceProvider(typeof(PdfDocument).Namespace));
            }
            else
            {
                var path = Path.Combine(_pathManager.ScriptsDir, "lib", moduleRequest.CmdArgument);
                if (File.Exists(path))
                {
                    var m = new Module(File.ReadAllText(path));

                    result = m;
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }

            result = mb.Build(moduleRequest.CmdArgument);
            return true;
        }
    }
}
