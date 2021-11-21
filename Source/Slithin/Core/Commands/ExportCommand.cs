using System;
using System.Linq;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Exporting;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Services;

namespace Slithin.Core.Commands;

public class ExportCommand : ICommand
{
    private readonly IExportProviderFactory _exportProviderFactory;

    public ExportCommand(IExportProviderFactory exportProviderFactory)
    {
        _exportProviderFactory = exportProviderFactory;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter is Metadata {Type: "DocumentType"} md &&
               _exportProviderFactory.GetAvailableProviders(md).Any();
    }

    public async void Execute(object parameter)
    {
        var md = (Metadata)parameter;

        var outputPath = await DialogService.ShowPrompt("Export", "Enter the path to export to");

        var provider = _exportProviderFactory.GetExportProvider("SVG Graphics");
        var options = new ExportOptions();
        var notebook = Notebook.Load(md);
        options.Document = notebook;
        options.PagesIndices = PageRange.ToIndices(PageRange.Parse("1-").AsT0, notebook.Pages.Count).ToList();


        provider.Export(options, md, outputPath);
    }
}
