using System;
using System.Collections.ObjectModel;
using System.Linq;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Messages;

namespace Slithin.Core.Sync
{
    public static class SyncQueueExtensions
    {
        public static void AnalyseAndAppend(this ObservableCollection<SyncItem> queue)
        {
            var mailboxService = ServiceLocator.Container.Resolve<IMailboxService>();

            var templates = queue.Where(_ => _.Direction == SyncDirection.ToDevice && _.Type == SyncType.Template);
            var importedNotebooks = queue.Where(_ => _.Direction == SyncDirection.ToDevice && _.Type == SyncType.Notebook);

            // if (templates.Any() || importedNotebooks.Any())
            {
                var applyTemplateAction = new Action<object>(_ =>
                {
                    TemplateStorage.Instance.Apply();
                });

                mailboxService.Post(new AttentionRequiredMessage { Title = "Apply Changes", Text = "All files are synced. Would you like to apply all changes?", Action = applyTemplateAction });
            }

            mailboxService.PostAction(() => NotificationService.Hide());
        }
    }
}
