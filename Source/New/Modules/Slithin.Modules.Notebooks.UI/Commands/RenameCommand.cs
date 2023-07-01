using System.Windows.Input;
using AuroraModularis.Logging.Models;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

[Context(UIContext.Notebook)]
internal class RenameCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;
    private readonly IDialogService _dialogService;
    private readonly NotebooksFilter _notebooksFilter;
    private readonly IMetadataRepository _metadataRepository;

    public RenameCommand(ILocalisationService localisationService,
                         ILogger logger, IDialogService dialogService,
                         NotebooksFilter notebooksFilter,
                         IMetadataRepository metadataRepository)
    {
        _localisationService = localisationService;
        _logger = logger;
        _dialogService = dialogService;
        _notebooksFilter = notebooksFilter;
        _metadataRepository = metadataRepository;
        
        _notebooksFilter.SelectionChanged += (s) =>
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }
    public TranslatedString Titel => "Rename";

    public bool CanExecute(object parameter)
    {
        return _notebooksFilter.Selection is FileSystemModel fsm && fsm.Tag is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && fsm is not UpDirectoryModel
               && fsm is not TrashModel;
    }

    public async void Execute(object parameter)
    {
        var name = await _dialogService.ShowPrompt("Rename",  "Name", ((Metadata)parameter).VisibleName);

        if (!string.IsNullOrEmpty(name))
        {
            Rename((Metadata)_notebooksFilter.Selection.Tag, name);
        }
    }

    private void Rename(Metadata md, string newName)
    {
        _logger.Info($"Renamed '{md.VisibleName}' to '{newName}'");

        md.VisibleName = newName;

        _metadataRepository.Remove(md);
        _metadataRepository.AddMetadata(md, out var alreadyAdded);

        if (alreadyAdded)
        {
            return;
        }

        _metadataRepository.SaveToDisk(md);
        _metadataRepository.Upload(md);
    }
}
