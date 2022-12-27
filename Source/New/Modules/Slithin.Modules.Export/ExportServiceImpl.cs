using AuroraModularis.Core;
using EpubSharp;
using PdfSharpCore.Pdf.IO;
using Slithin.Core.Services;
using Slithin.Entities.Remarkable;
using Slithin.Entities.Remarkable.Rendering;
using Slithin.Modules.Export.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Export;

internal class ExportServiceImpl : IExportService
{
    public ExportServiceImpl(Container container)
    {
        Container = container;
    }

    public Container Container { get; set; }

    public async Task Export(Metadata metadata)
    {
        var exportProviderFactory = Container.Resolve<IExportProviderFactory>();
        var dialogService = Container.Resolve<IDialogService>();
        var localisationService = Container.Resolve<ILocalisationService>();
        var notificationService = Container.Resolve<INotificationService>();

        var modal = new ExportModal();
        var vm = new ExportModalViewModel(metadata, exportProviderFactory);
        modal.DataContext = vm;

        if (await dialogService.Show(localisationService.GetString("Export"), modal))
        {
            /*var validationResult = _validator.Validate(vm);

            if (!validationResult.IsValid)
            {
                NotificationService.ShowError(string.Join("\n", validationResult.Errors));
                return;
            }
            */

            var provider = vm.SelectedFormat;

            ExportOptions options = null;
            if (metadata.Content.FileType == "notebook")
            {
                var notebook = Notebook.Load(metadata);
                options = ExportOptions.Create(notebook, vm.PagesSelector);
            }
            else if (metadata.Content.FileType == "pdf")
            {
                var pathManager = Container.Current.Resolve<IPathManager>();
                var path = Path.Combine(pathManager.NotebooksDir, metadata.ID + ".pdf");

                var pdfStream = File.OpenRead(path);
                var notebook = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
                options = ExportOptions.Create(notebook, vm.PagesSelector);
            }
            else if (metadata.Content.FileType == "epub")
            {
                var pathManager = Container.Current.Resolve<IPathManager>();
                var path = Path.Combine(pathManager.NotebooksDir, metadata.ID + ".epub");

                var notebook = EpubReader.Read(path);
                options = ExportOptions.Create(notebook, vm.PagesSelector);
            }

            var status = notificationService.ShowStatus("");
            await Task.Run(() =>
            {
                var progress = new Progress<int>();

                progress.ProgressChanged += (s, e) =>
                {
                    status.Step(
                        localisationService.GetStringFormat("Exporting {0}", metadata.VisibleName));
                };

                if (!Directory.Exists(vm.ExportPath))
                {
                    Directory.CreateDirectory(vm.ExportPath);
                }

                provider.Export(options, metadata, vm.ExportPath, progress);
            });

            status.Step(localisationService.GetStringFormat("{0} Exported", metadata.VisibleName));
        }
    }
}
