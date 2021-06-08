using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.Remarkable;

namespace Slithin.ViewModels
{
    public class TemplatesPageViewModel : BaseViewModel
    {
        public ObservableCollection<Template> Templates { get; set; }


        public ICommand RefreshCommand { get; set; }

        public ObservableCollection<string> Categories { get; set; }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); }
        }


        public TemplatesPageViewModel()
        {
            RefreshCommand = new DelegateCommand(Refresh);
            Templates = new();
            Categories = new();
        }

        private void Refresh(object? obj)
        {
            Templates.Clear();

            TemplateStorage.Instance?.Load();

            var tempCats = TemplateStorage.Instance?.Templates.Select(_ => _.Categories);
            foreach (var item in tempCats)
            {
                foreach (var cat in item)
                {
                    if (!Categories.Contains(cat))
                    {
                        Categories.Add(cat);
                    }
                }
            }

            foreach (var item in TemplateStorage.Instance?.Templates)
            {
                Templates.Add(item);
            }


        }
    }
}