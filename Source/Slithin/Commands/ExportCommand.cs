using System;
using System.Linq;
using System.Windows.Input;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Services;
using Slithin.Core.Remarkable.Models;
using Slithin.Core;

namespace Slithin.Commands;

public class ExportCommand : ICommand
{
    private readonly IExportProviderFactory _exportProviderFactory;
    private readonly ILocalisationService _localisationService;

    public ExportCommand(IExportProviderFactory exportProviderFactory, ILocalisationService localisationService)
    {
        _exportProviderFactory = exportProviderFactory;
        _localisationService = localisationService;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter is Metadata { Type: "DocumentType" } md &&
               _exportProviderFactory.GetAvailableProviders(md).Any();
    }

    public async void Execute(object parameter)
    {
        var md = (Metadata)parameter;

        var outputPath = await DialogService.ShowPrompt(_localisationService.GetString("Export"),
            _localisationService.GetString("Enter the path to export to"));

        var provider = _exportProviderFactory.GetExportProvider("SVG Graphics");

        var notebook = Notebook.Load(md);
        var options = ExportOptions.Create(notebook, "1-120");

        provider.Export(options, md, outputPath);
    }
}
