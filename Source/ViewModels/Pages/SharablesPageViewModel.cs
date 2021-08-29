using System.Collections.ObjectModel;
using Slithin.Core;
using Slithin.Models;

namespace Slithin.ViewModels.Pages
{
    public class SharablesPageViewModel : BaseViewModel
    {
        public SharablesPageViewModel()
        {
            Items.Add(new() { ID = "1", IsInstalled = false, Name = "Not Installed Template 1" });
            Items.Add(new() { ID = "2", IsInstalled = true, Name = "Installed Template 2" });
            Items.Add(new() { ID = "3", IsInstalled = false, Name = "Installed Template 3" });
            Items.Add(new() { ID = "4", IsInstalled = true, Name = "Installed Template 4" });
        }

        public ObservableCollection<Sharable> Items { get; set; } = new();
    }
}
