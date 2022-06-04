using Ionic.Zip;
using Slithin.Core;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;

namespace Slithin.Commands.ContextCommands.Notebooks;

[Context(UIContext.Notebook)]
public class RestoreNotebookContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;

    public RestoreNotebookContextCommand(ILocalisationService localisationService,
        IPathManager pathManager,
        IMailboxService mailboxService)
    {
        _localisationService = localisationService;
        _pathManager = pathManager;
        _mailboxService = mailboxService;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Restore");

    public bool CanExecute(object data)
    {
        return data is Metadata md && md.VisibleName != _localisationService.GetString("Trash");
    }

    public void Execute(object data)
    {
        Restore((Metadata)data);
    }

    private void Restore(Metadata md)
    {
        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetStringFormat("Start Compressing {0}", md.VisibleName));

            var zip = new ZipFile();

            //ToDo: implement restore single notebook
            //BackupNotebookOrFolder(md, zip);

            zip.ExtractProgress += (s, e) =>
            {
                if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                {
                    NotificationService.ShowProgress(_localisationService.GetStringFormat("Compressing '{0}'", e.CurrentEntry.FileName), e.EntriesExtracted, e.EntriesTotal);
                }
            };

            zip.Dispose();
        });
    }
}
