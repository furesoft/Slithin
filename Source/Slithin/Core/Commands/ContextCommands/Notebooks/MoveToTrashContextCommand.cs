using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Commands.ContextCommands;

namespace Slithin.Core.Commands.ContextCommands.Notebooks;

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

    public bool CanHandle(object data)
    {
        return data != null
                && data is Metadata md
                && md.VisibleName != _localisationService.GetString("Quick sheets")
                && md.VisibleName != _localisationService.GetString("Up ..")
                && md.VisibleName != _localisationService.GetString("Trash");
    }

    public void Invoke(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        MetadataStorage.Local.Move(md, "trash");

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
    }
}
