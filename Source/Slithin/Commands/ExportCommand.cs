using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using EpubSharp;
using PdfSharpCore.Pdf.IO;
using Slithin.Core;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.UI.Modals;
using Slithin.ViewModels.Modals;

namespace Slithin.Commands;

public class ExportCommand : ICommand
{
    private readonly IExportProviderFactory _exportProviderFactory;
    private readonly ILocalisationService _localisationService;
    private readonly IMailboxService _mailboxService;

    public ExportCommand(IExportProviderFactory exportProviderFactory,
                         ILocalisationService localisationService,
                         IMailboxService mailboxService)
    {
        _exportProviderFactory = exportProviderFactory;
        _localisationService = localisationService;
        _mailboxService = mailboxService;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && md.VisibleName != _localisationService.GetString("Up ..")
               && md.VisibleName != _localisationService.GetString("Trash")
               && md.Type.Equals("DocumentType")
               && _exportProviderFactory.GetAvailableProviders(md).Any();
    }

    public async void Execute(object parameter)
    {
        var md = (Metadata)parameter;

        var modal = new ExportModal();
        var vm = new ExportModalViewModel(md, ServiceLocator.Container.Resolve<IExportProviderFactory>());
        modal.DataContext = vm;

        if (await DialogService.ShowDialog(_localisationService.GetString("Export"), modal))
        {
            var provider = vm.SelectedFormat;

            ExportOptions options = null;
            if (md.Content.FileType == "notebook")
            {
                var notebook = Notebook.Load(md);
                options = ExportOptions.Create(notebook, vm.PagesSelector);
            }
            else if (md.Content.FileType == "pdf")
            {
                var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
                var path = Path.Combine(pathManager.NotebooksDir, md.ID + ".pdf");

                var pdfStream = File.OpenRead(path);
                var notebook = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Modify);
                options = ExportOptions.Create(notebook, vm.PagesSelector);
            }
            else if (md.Content.FileType == "epub")
            {
                var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
                var path = Path.Combine(pathManager.NotebooksDir, md.ID + ".epub");

                var notebook = EpubReader.Read(path);
                options = ExportOptions.Create(notebook, vm.PagesSelector);
            }

            _mailboxService.PostAction(() =>
            {
                var progress = new Progress<int>();

                progress.ProgressChanged += (s, e) =>
                {
                    NotificationService.ShowProgress(
                        _localisationService.GetStringFormat("Exporting {0}", md.VisibleName), e, 100);
                };

                if (!Directory.Exists(vm.ExportPath))
                {
                    Directory.CreateDirectory(vm.ExportPath);
                }

                provider.Export(options, md, vm.ExportPath, progress);

                NotificationService.Show(_localisationService.GetStringFormat("{0} Exported", md.VisibleName));
            });
        }
    }
}
