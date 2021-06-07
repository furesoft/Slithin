using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Remarkable;

namespace Slithin.ViewModels
{
    public class TemplatesPageViewModel : BaseViewModel
    {
        public ObservableCollection<Template> Templates { get; set; }


        public ICommand RefreshCommand { get; set; }

        public TemplatesPageViewModel()
        {
            RefreshCommand = new DelegateCommand(Refresh);
            Templates = new();
        }

        private void Refresh(object? obj)
        {
            Templates.Clear();

            TemplateStorage.Instance?.Load();

            foreach (var item in TemplateStorage.Instance?.Templates)
            {
                Templates.Add(item);
            }
        }
    }
}