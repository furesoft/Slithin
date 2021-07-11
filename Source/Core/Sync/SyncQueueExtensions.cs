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
            //ToDo: check if any templates are synchronized->append message to show dialog to apply all templates
            var templates = queue.FindAll().Where(_ => _.Direction == SyncDirection.ToDevice && _.Type == SyncType.Template);

            if (templates.Any())
            {
                var applyTemplateAction = new Action<object>(_ =>
                {
                    TemplateStorage.Instance.Apply();
                });

                ServiceLocator.Mailbox.Post(new AttentionRequiredMessage { Title = "Apply Templates", Text = "All Templates are synced. Would you like to apply all templates?", Action = applyTemplateAction });
            }

            ServiceLocator.Mailbox.Post(new HideStatusMessage());
        }
    }
}
