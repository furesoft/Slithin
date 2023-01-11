using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

[Context(UIContext.Notebook)]
internal class RemoveNotebookCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IDialogService _dialogService;
    private readonly IMetadataRepository _metadataRepository;
    private readonly NotebooksFilter _notebooksFilter;

    public RemoveNotebookCommand(ILocalisationService localisationService,
                                 IDialogService dialogService, IMetadataRepository metadataRepository,
                                 NotebooksFilter notebooksFilter)
    {
        _localisationService = localisationService;
        _dialogService = dialogService;
        _metadataRepository = metadataRepository;
        _notebooksFilter = notebooksFilter;
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Remove");

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is FileSystemModel fsm || fsm.Tag is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && fsm is not UpDirectoryModel
               && fsm is not TrashModel;
    }

    public async void Execute(object parameter)
    {
        if (parameter is not FileSystemModel fsm || fsm.Tag is not Metadata md
            || !await _dialogService.Show(
                _localisationService.GetStringFormat("Would you really want to delete '{0}'?", md.VisibleName)))
            return;

        _notebooksFilter.SelectedNotebook = null;

        _metadataRepository.Remove(md);
        //_localRepository.Remove(md);
        //_deviceRepository.Remove(md);

        _notebooksFilter.Documents.Remove(_notebooksFilter.Documents.First(_ => _.ID == md.ID));

        _notebooksFilter.SortByFolder();
    }
}
