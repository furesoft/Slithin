using System.Windows.Input;
using AuroraModularis.Logging.Models;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Notebooks.UI.Commands;

[Context(UIContext.Notebook)]
internal class RenameCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;

    public RenameCommand(ILocalisationService localisationService, ILogger logger)
    {
        _localisationService = localisationService;
        _logger = logger;
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
        /*
        var name = await DialogService.ShowPrompt(_localisationService.GetString("Rename"),
                _localisationService.GetString("Name"), ((Metadata)parameter).VisibleName);

        if (!string.IsNullOrEmpty(name))
        {
            Rename((Metadata)parameter, name);
        }*/
    }

    private void Rename(Metadata md, string newName)
    {
        _logger.Info($"Renamed '{md.VisibleName}' to '{newName}'");

        /*
        md.VisibleName = newName;

        MetadataStorage.Local.Remove(md);
        MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);

        if (alreadyAdded)
        {
            return;
        }

        md.Save();

        md.Upload();

        DialogService.Close();
        */
    }
}
