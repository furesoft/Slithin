using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.Modals;

namespace Slithin.ViewModels
{
    public class TemplatesPageViewModel : BaseViewModel
    {
        private Template _selectedTemplate
            ;

        public TemplatesPageViewModel()
        {
            OpenAddModalCommand = DialogService.CreateOpenCommand(new AddTemplateModal(), new AddTemplateModalViewModel());
            RemoveTemplateCommand = new RemoveTemplateCommand(this);
        }

        public ICommand OpenAddModalCommand { get; set; }

        public ICommand RemoveTemplateCommand { get; set; }

        public Template SelectedTemplate
        {
            get { return _selectedTemplate; }
            set { SetValue(ref _selectedTemplate, value); }
        }
    }
}
