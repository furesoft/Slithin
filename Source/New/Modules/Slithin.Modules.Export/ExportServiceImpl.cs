using AuroraModularis.Core;
using EpubSharp;
using PdfSharpCore.Pdf.IO;
using Slithin.Core;
using Slithin.Entities.Remarkable;
using Slithin.Entities.Remarkable.Rendering;
using Slithin.Modules.Export.Models;
using Slithin.Modules.Export.Validators;
using Slithin.Modules.Export.ViewModels;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Export;

internal class ExportServiceImpl : IExportService
{
    public ExportServiceImpl(Container container)
    {
        this._container = container;
    }

    private readonly Container _container;

    public async Task Export(Metadata metadata)
    {
        var exportProviderFactory = _container.Resolve<IExportProviderFactory>();
        var dialogService = _container.Resolve<IDialogService>();
        var localisationService = _container.Resolve<ILocalisationService>();
        var notificationService = _container.Resolve<INotificationService>();
        var validator = _container.Resolve<ExportValidator>();

        var modal = new ExportModal();
        var vm = new ExportModalViewModel(metadata, exportProviderFactory);
        modal.DataContext = vm;
        
        var shouldExport = await dialogService.Show(localisationService.GetString("Export"), modal);
        if (!shouldExport)
        {
            return;
        }

        var validationResult = await validator.ValidateAsync(vm);
        if (!validationResult.IsValid)
        {
            notificationService.ShowError(validationResult.Errors.AsString());
            return;
        }

        if (await DeleteDirectoryIfNessesary(vm, dialogService, localisationService))
        {
            return;
        }
        
        var options = InitExportOptions(metadata, vm);

        await DoExport(metadata, notificationService, localisationService, vm, vm.SelectedFormat, options);
    }

    private static async Task DoExport(Metadata metadata, INotificationService notificationService,
        ILocalisationService localisationService, ExportModalViewModel vm, IExportProvider provider, ExportOptions? options)
    {
        await Task.Run(() =>
        {
            var status = notificationService.ShowStatus("");
            var progress = new Progress<int>();

            progress.ProgressChanged += (s, e) =>
            {
                status.Step(
                    localisationService.GetStringFormat("Exporting {0}", $"{metadata.VisibleName} {e} %"));
            };

            if (!Directory.Exists(vm.ExportPath))
            {
                Directory.CreateDirectory(vm.ExportPath);
            }

            provider.Export(options, metadata, vm.ExportPath, progress);

            status.Step(localisationService.GetStringFormat("{0} Exported", metadata.VisibleName));
        });
    }

    private static async Task<bool> DeleteDirectoryIfNessesary(ExportModalViewModel vm, IDialogService dialogService,
        ILocalisationService localisationService)
    {
        if (!Directory.Exists(vm.ExportPath))
        {
            return false;
        }

        if (!await dialogService.Show(
                localisationService.GetStringFormat("Directory already exists. Do you want to override its content?")))
        {
            return true;
        }

        Directory.Delete(vm.ExportPath, true);
        Directory.CreateDirectory(vm.ExportPath);

        return true;

    }

    private static ExportOptions? InitExportOptions(Metadata metadata, ExportModalViewModel vm)
    {
        var options = metadata.Content.FileType switch
        {
            "notebook" => CreateNotebookOptions(metadata, vm),
            "pdf" => CreatePdfOptions(metadata, vm),
            "epub" => CreateEpubOptions(metadata, vm),
            _ => null
        };

        return options;
    }

    private static ExportOptions CreateNotebookOptions(Metadata metadata, ExportModalViewModel vm)
    {
        var notebook = Notebook.Load(metadata);
        var options = ExportOptions.Create(notebook, vm.PagesSelector);
        
        return options;
    }

    private static ExportOptions CreatePdfOptions(Metadata metadata, ExportModalViewModel vm)
    {
        var pathManager = Container.Current.Resolve<IPathManager>();
        var path = Path.Combine(pathManager.NotebooksDir, metadata.ID + ".pdf");

        var pdfStream = File.OpenRead(path);
        var notebook = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
        var options = ExportOptions.Create(notebook, vm.PagesSelector);
        
        return options;
    }

    private static ExportOptions CreateEpubOptions(Metadata metadata, ExportModalViewModel vm)
    {
        var pathManager = Container.Current.Resolve<IPathManager>();
        var path = Path.Combine(pathManager.NotebooksDir, metadata.ID + ".epub");

        var notebook = EpubReader.Read(path);
        var options = ExportOptions.Create(notebook, vm.PagesSelector);
        
        return options;
    }
}
