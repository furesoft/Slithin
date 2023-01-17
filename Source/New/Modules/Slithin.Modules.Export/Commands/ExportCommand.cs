using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Export.Commands;

internal class ExportCommand : ICommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IExportService _exportService;
    private readonly NotebooksFilter _filter;

    public ExportCommand(ILocalisationService localisationService, IExportService exportService,
        NotebooksFilter filter)
    {
        _localisationService = localisationService;
        _exportService = exportService;
        _filter = filter;

        _filter.SelectionChanged += (s) =>
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return _filter.Selection is not null
               && _filter.Selection.Tag is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && _filter.Selection is not UpDirectoryModel
               && _filter.Selection is not TrashModel
               && _filter.Selection is FileModel;
        //&& _exportProviderFactory.GetAvailableProviders(md).Any();
    }

    public async void Execute(object parameter)
    {
        await _exportService.Export((Metadata)_filter.Selection.Tag);
    }
}
