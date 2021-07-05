using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Messages;

namespace Slithin.ViewModels
{
    public class AddTemplateModalViewModel : BaseViewModel
    {
        private string _selectedCategory;

        public AddTemplateModalViewModel()
        {
            Categories = SyncService.Categories;
            Categories.RemoveAt(0);

            SelectedCategory = "Grids";

            foreach (var res in typeof(IconCodeItem).Assembly.GetManifestResourceNames())
            {
                if (res.StartsWith("Slithin.Resources.IconTiles."))
                {
                    var item = new IconCodeItem { Name = res.Split('.')[^2] };
                    item.Load();

                    IconCodes.Add(item);
                }
            }

            AddTemplateCommand = new DelegateCommand(AddTemplate);
        }

        public ICommand AddTemplateCommand { get; set; }

        public ObservableCollection<string> Categories { get; set; }

        public ObservableCollection<IconCodeItem> IconCodes { get; set; } = new();

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); }
        }

        private void AddTemplate(object obj)
        {
            ServiceLocator.Mailbox.Post(new UploadTemplateMessage());
        }
    }
}
