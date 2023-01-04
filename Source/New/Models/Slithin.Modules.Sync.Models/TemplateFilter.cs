using System.Collections.ObjectModel;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Sync.Models;

public class TemplatesFilter : NotifyObject
{
    private bool _landscape;
    private string _selectedCategory = string.Empty;

    private ObservableCollection<Template> _templates = new();

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

    public ObservableCollection<Template> Templates
    {
        get => _templates;
        set { SetValue(ref _templates, value); }
    }

    private void RefreshTemplates()
    {
        //Templates = new(TemplateStorage.Instance?.Templates.Where(_ => _.Categories.Contains(SelectedCategory) && Landscape == _.Landscape));
    }
}
