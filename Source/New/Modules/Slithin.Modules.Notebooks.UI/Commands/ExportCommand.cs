using System.Windows.Input;
using AuroraModularis.Core;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI.Commands;

internal class ExportCommand : ICommand
{
    //private readonly IExportProviderFactory _exportProviderFactory;
    private readonly ILocalisationService _localisationService;

    //private readonly ExportValidator _validator;

    public ExportCommand(//IExportProviderFactory exportProviderFactory,
                         ILocalisationService localisationService
                         //ExportValidator validator
                         )
    {
        //_exportProviderFactory = exportProviderFactory;
        _localisationService = localisationService;
        //_validator = validator;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && md.VisibleName != _localisationService.GetString("Up ..")
               && md.VisibleName != _localisationService.GetString("Trash")
               && md.Type.Equals("DocumentType");
        //&& _exportProviderFactory.GetAvailableProviders(md).Any();
    }

    public async void Execute(object parameter)
    {
        var md = (Metadata)parameter;

        Container.Current.Resolve<IRenderingService>().RenderSvg(new(), 0, md);
        /*
        var modal = new ExportModal();
        var vm = new ExportModalViewModel(md, ServiceLocator.Container.Resolve<IExportProviderFactory>());
        modal.DataContext = vm;

        if (await DialogService.ShowDialog(_localisationService.GetString("Export"), modal))
        {
            var validationResult = _validator.Validate(vm);

            if (!validationResult.IsValid)
            {
                NotificationService.ShowError(string.Join("\n", validationResult.Errors));
                return;
            }

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
        */
    }
}
