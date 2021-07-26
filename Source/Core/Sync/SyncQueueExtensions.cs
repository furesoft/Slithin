using System;
using System.Linq;
using Slithin.Core.Remarkable;
using Slithin.Messages;

namespace Slithin.Core.Sync
{
    public static class SyncQueueExtensions
    {
        public static void AnalyseAndAppend(this LiteDB.ILiteCollection<SyncItem> queue)
        {
            var templates = queue.FindAll().Where(_ => _.Direction == SyncDirection.ToDevice && _.Type == SyncType.Template);
            var importedNotebooks = queue.FindAll().Where(_ => _.Direction == SyncDirection.ToDevice && _.Type == SyncType.Notebook);

            if (templates.Any() || importedNotebooks.Any())
            {
                var applyTemplateAction = new Action<object>(_ =>
                {
                    TemplateStorage.Instance.Apply();
                });

                ServiceLocator.Mailbox.Post(new AttentionRequiredMessage { Title = "Apply Changes", Text = "All files are synced. Would you like to apply all changes?", Action = applyTemplateAction });
            }

            ServiceLocator.Mailbox.Post(new HideStatusMessage());
        }
    }
}
