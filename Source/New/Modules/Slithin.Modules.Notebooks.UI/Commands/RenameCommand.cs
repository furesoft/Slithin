using System.Windows.Input;
using AuroraModularis.Logging.Models;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

[Context(UIContext.Notebook)]
internal class RenameCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;
    private readonly IDialogService _dialogService;
    private readonly IMetadataRepository _metadataRepository;

    public RenameCommand(ILocalisationService localisationService,
                         ILogger logger, IDialogService dialogService,
                         IMetadataRepository metadataRepository)
    {
        _localisationService = localisationService;
        _logger = logger;
        _dialogService = dialogService;
        _metadataRepository = metadataRepository;
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Rename");

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && md.VisibleName != _localisationService.GetString("Up ..")
               && md.VisibleName != _localisationService.GetString("Trash");
    }

    public async void Execute(object parameter)
    {
        var name = await _dialogService.ShowPrompt(_localisationService.GetString("Rename"),
                _localisationService.GetString("Name"), ((Metadata)parameter).VisibleName);

        if (!string.IsNullOrEmpty(name))
        {
            Rename((Metadata)parameter, name);
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
