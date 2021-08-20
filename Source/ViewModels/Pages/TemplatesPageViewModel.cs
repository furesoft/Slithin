using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.UI.Modals;
using Slithin.ViewModels.Modals;
using Slithin.Core.Services;

namespace Slithin.ViewModels.Pages
{
    public class TemplatesPageViewModel : BaseViewModel
    {
        private readonly ILoadingService _loadingService;
        private readonly IMailboxService _mailboxService;

        private Template _selectedTemplate;

        public TemplatesPageViewModel(ILoadingService loadingService, IMailboxService mailboxService)
        {
            OpenAddModalCommand = DialogService.CreateOpenCommand<AddTemplateModal>(ServiceLocator.Container.Resolve<AddTemplateModalViewModel>());
            RemoveTemplateCommand = ServiceLocator.Container.Resolve<RemoveNotebookCommand>();
            _loadingService = loadingService;
            _mailboxService = mailboxService;
        }

        public ICommand OpenAddModalCommand { get; set; }

        public ICommand RemoveTemplateCommand { get; set; }

        public Template SelectedTemplate
        {
            get { return _selectedTemplate; }
            set { _selectedTemplate = value; }
        }

        public override void OnLoad()
        {
            base.OnLoad();

            _mailboxService.PostAction(() =>
            {
                NotificationService.Show("Loading Templates");

                _loadingService.LoadTemplates();

                SyncService.TemplateFilter.SelectedCategory = "Grids";

                NotificationService.Hide();
            });
        }
    }
}
