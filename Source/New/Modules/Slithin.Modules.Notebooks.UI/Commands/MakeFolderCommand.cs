using System.Windows.Input;
using AuroraModularis.Logging.Models;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class MakeFolderCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;
    private readonly NotebooksFilter _notebooksFilter;
    private readonly IMetadataRepository _metadataRepository;
    private readonly IDialogService _dialogService;

    public MakeFolderCommand(ILocalisationService localisationService,
                             ILogger logger, NotebooksFilter notebooksFilter, IMetadataRepository metadataRepository,
                             IDialogService dialogService)
    {
        _localisationService = localisationService;
        _logger = logger;
        _notebooksFilter = notebooksFilter;
        _metadataRepository = metadataRepository;
        _dialogService = dialogService;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public async void Execute(object parameter)
    {
        var name = await _dialogService.ShowPrompt("Make Folder", "Name");

        if (!string.IsNullOrEmpty(name))
        {
            MakeFolder(name);
        }
    }

    private void MakeFolder(string name)
    {
        var id = Guid.NewGuid().ToString().ToLower();

        var md = new Metadata
        {
            ID = id,
            Parent = _notebooksFilter.Folder,
            Type = "CollectionType",
            VisibleName = name
        };

        _metadataRepository.AddMetadata(md, out var alreadyAdded);

        if (alreadyAdded)
        {
            _dialogService.Show(_localisationService.GetStringFormat("'{0}' already exists", md.VisibleName));
            return;
        }

        _metadataRepository.SaveToDisk(md);

        _notebooksFilter.Items.Add(new Models.DirectoryModel(md.VisibleName, md, md.IsPinned));
        _notebooksFilter.SortByFolder();

        _metadataRepository.Upload(md);

        _logger.Info($"Folder '{md.VisibleName}' created");
    }
}
