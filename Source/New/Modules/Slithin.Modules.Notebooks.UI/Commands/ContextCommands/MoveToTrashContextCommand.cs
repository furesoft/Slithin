using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.Commands.ContextCommands;

[Context(UIContext.Notebook)]
internal class MoveToTrashContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly NotebooksFilter _notebooksFilter;
    private readonly IMetadataRepository _metadataRepository;

    public MoveToTrashContextCommand(ILocalisationService localisationService,
                                     NotebooksFilter notebooksFilter,
                                     IMetadataRepository metadataRepository)
    {
        _localisationService = localisationService;
        _notebooksFilter = notebooksFilter;
        _metadataRepository = metadataRepository;
    }

    public object ParentViewModel { get; set; }
    public TranslatedString Title => "Move To Trash";

    public bool CanExecute(object data)
    {
        return data is Metadata md
                && md.VisibleName != _localisationService.GetString("Quick sheets")
                && md.VisibleName != _localisationService.GetString("Up ..")
                && md.VisibleName != _localisationService.GetString("Trash");
    }

    public void Execute(object data)
    {
        if (data is not FileSystemModel fsm || fsm.Tag is not Metadata md)
        {
            return;
        }

        _metadataRepository.Move(md, "trash");

        if (fsm is DirectoryModel)
        {
            foreach (var childMd in _metadataRepository.GetByParent(md.ID))
            {
                _metadataRepository.Move(childMd, "trash");
            }
        }

        _notebooksFilter.Items.Remove(fsm);
    }
}
