using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Modals;

namespace Slithin.ViewModels
{
    public class TemplatesPageViewModel : BaseViewModel
    {
        public ICommand OpenAddModalCommand { get; set; }

        public ICommand AddToQueueCommand { get; set; }

        public TemplatesPageViewModel()
        {
            OpenAddModalCommand = DialogService.CreateOpenCommand(new AddTemplateModal(), new AddTemplateModalViewModel());
            AddToQueueCommand = new AddToSyncQueueCommand();
        }
    }
}