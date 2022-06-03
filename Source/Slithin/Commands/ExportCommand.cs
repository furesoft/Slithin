using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.FeatureToggle;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Features;
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
        return Feature<ExportFeature>.IsEnabled && parameter is Metadata { Type: "DocumentType" } md &&
               _exportProviderFactory.GetAvailableProviders(md).Any();
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

            var notebook = Notebook.Load(md);
            var options = ExportOptions.Create(notebook, vm.PagesSelector);

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
