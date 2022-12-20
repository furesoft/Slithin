using System.Windows.Input;
using AuroraModularis.Logging.Models;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class MakeFolderCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;

    public MakeFolderCommand(ILocalisationService localisationService, ILogger logger)
    {
        _localisationService = localisationService;
        _logger = logger;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public async void Execute(object parameter)
    {
        /*
        var name = await DialogService.ShowPrompt(_localisationService.GetString("Make Folder"),
                _localisationService.GetString("Name"));

        if (!string.IsNullOrEmpty(name))
        {
            MakeFolder(name);
        }*/
    }

    private void MakeFolder(string name)
    {
        var id = Guid.NewGuid().ToString().ToLower();

        /*
        var md = new Metadata
        {
            ID = id,
            Parent = ServiceLocator.SyncService.NotebooksFilter.Folder,
            Type = "CollectionType",
            VisibleName = name
        };

        MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);

        if (alreadyAdded)
        {
            DialogService.OpenError(_localisationService.GetStringFormat("'{0}' already exists", md.VisibleName));
            return;
        }

        md.Save();

        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);
        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

        md.Upload();

        _logger.Information($"Folder '{md.VisibleName}' created");

        DialogService.Close();
        */
    }
}
