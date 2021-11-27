using System.Collections.ObjectModel;
using Slithin.Core;

namespace Slithin.ViewModels.Modals;

public class NewScriptModalViewModel : BaseViewModel
{
    private string _name;

    private string _selectedCategory;


    public NewScriptModalViewModel()
    {
        Categories = new ObservableCollection<string>(SyncService.ToolsFilter.Categories);
    }

    public ObservableCollection<string> Categories { get; set; }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set => SetValue(ref _selectedCategory, value);
    }

    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }
}
