using System.Collections.ObjectModel;
using Slithin.Core;
using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Sync.Models;

public class TemplatesFilter : FilterBase<Template>
{
    private bool _landscape;
    private string _selectedCategory = string.Empty;
    
    public ObservableCollection<string> Categories { get; set; } = new();

    public bool Landscape
    {
        get => _landscape;
        set { SetValue(ref _landscape, value); }
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set { SetValue(ref _selectedCategory, value); }
    }

    private void RefreshTemplates()
    {
        //Templates = new(TemplateStorage.Instance?.Templates.Where(_ => _.Categories.Contains(SelectedCategory) && Landscape == _.Landscape));
    }
}
