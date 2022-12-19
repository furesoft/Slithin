using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Notebooks.UI.Commands.ContextCommands;

[Context(UIContext.Notebook)]
public class MoveToTrashContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;

    public MoveToTrashContextCommand(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Move To Trash");

    public bool CanExecute(object data)
    {
        return data != null
                && data is Metadata md
                && md.VisibleName != _localisationService.GetString("Quick sheets")
                && md.VisibleName != _localisationService.GetString("Up ..")
                && md.VisibleName != _localisationService.GetString("Trash");
    }

    public void Execute(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        /*MetadataStorage.Local.Move(md, "trash");

        if (md.Type == "CollectionType")
        {
            foreach (var childMd in MetadataStorage.Local.GetByParent(md.ID))
            {
                MetadataStorage.Local.Move(childMd, "trash");
            }
        }

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        */
    }
}
