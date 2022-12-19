using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Notebooks.UI.Commands.ContextCommands;

[Context(UIContext.Notebook)]
public class RestoreNotebookContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;

    public RestoreNotebookContextCommand(ILocalisationService localisationService, IPathManager pathManager)
    {
        _localisationService = localisationService;
        _pathManager = pathManager;
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
        /*  NotificationService.Show(_localisationService.GetStringFormat("Start Compressing {0}", md.VisibleName));

          var zip = new ZipFile();

          //ToDo: implement restore single notebook

          zip.ExtractProgress += (s, e) =>
          {
              if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
              {
                  NotificationService.ShowProgress(_localisationService.GetStringFormat("Compressing '{0}'", e.CurrentEntry.FileName), e.EntriesExtracted, e.EntriesTotal);
              }
          };

          zip.Dispose();
      */
    }
}
