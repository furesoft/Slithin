using System.Collections.ObjectModel;
using Slithin.Core;

namespace Slithin.ViewModels
{
    public class AddTemplateModalViewModel : BaseViewModel
    {
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<IconCodeItem> IconCodes { get; set; } = new();
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
        }

        private string _selectedCategory;

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); }
        }
    }
}
