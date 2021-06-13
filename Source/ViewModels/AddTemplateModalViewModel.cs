using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Slithin.Core;

namespace Slithin.ViewModels
{
    public class AddTemplateModalViewModel : BaseViewModel
    {
        public ObservableCollection<string> Categories { get; set; }
        public AddTemplateModalViewModel()
        {
            Categories = SyncService.Categories;
            Categories.RemoveAt(0);

            SelectedCategory = "Grids";
        }

        private string _selectedCategory;

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); }
        }
    }
}
