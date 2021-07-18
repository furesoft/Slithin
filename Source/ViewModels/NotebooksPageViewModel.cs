using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;

namespace Slithin.ViewModels
{
    public class NotebooksPageViewModel : BaseViewModel
    {
        private Metadata _selectedNotebook;

        public NotebooksPageViewModel()
        {
            //ImportCommand = DialogService.CreateOpenCommand<ImportNotebookModal>(new AddTemplateModalViewModel());
            RemoveTemplateCommand = new RemoveNotebookCommand(this);
        }

        public ICommand ExportCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand RemoveTemplateCommand { get; set; }

        public Metadata SelectedNotebook
        {
            get { return _selectedNotebook; }
            set { SetValue(ref _selectedNotebook, value); }
        }
    }
}
