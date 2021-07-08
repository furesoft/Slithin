using System;
using System.Diagnostics;
using System.Windows.Input;
using Slithin.Core.Remarkable;
using Slithin.ViewModels;

namespace Slithin.Core.Commands
{
    public class RemoveTemplateCommand : ICommand
    {
        private TemplatesPageViewModel _templatesPageViewModel;

        public RemoveTemplateCommand(TemplatesPageViewModel templatesPageViewModel)
        {
            _templatesPageViewModel = templatesPageViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter != null && parameter is Template;
        }

        public void Execute(object parameter)
        {
            if (parameter is Template tmpl)
            {
                //ToDo: display template delete confirm modal
                // if ok, remove from templatestorage...
                _templatesPageViewModel.SelectedTemplate = null;
                ServiceLocator.SyncService.Templates.Remove(tmpl);
            }
        }
    }
}
