using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class ExportCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IExportService _exportService;

    //private readonly ExportValidator _validator;

    public ExportCommand(ILocalisationService localisationService, IExportService exportService
                         //ExportValidator validator
                         )
    {
        _localisationService = localisationService;
        _exportService = exportService;
        //_validator = validator;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is FileSystemModel fsm || fsm.Tag is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && fsm is not UpDirectoryModel
               && fsm is not TrashModel
               && md.Type.Equals("DocumentType");
        //&& _exportProviderFactory.GetAvailableProviders(md).Any();
    }

    public async void Execute(object parameter)
    {
        var md = (Metadata)parameter;

        await _exportService.Export(md);
    }
}
