using System.Collections.ObjectModel;
using System.Linq;
using Slithin.Core.Remarkable;
using Slithin.Core.MVVM;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Core.Sync;

public class TemplateFilter : NotifyObject
{
    private bool _landscape;
    private string _selectedCategory;

    private ObservableCollection<Template> _templates;

    public TemplateFilter()
    {
        Categories = new();

        Templates = new();
    }

    public ObservableCollection<string> Categories { get; set; }

    public bool Landscape
    {
        get => _landscape;
        set { SetValue(ref _landscape, value); RefreshTemplates(); }
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set { SetValue(ref _selectedCategory, value); RefreshTemplates(); }
    }

    public ObservableCollection<Template> Templates
    {
        get => _templates;
        set { SetValue(ref _templates, value); }
    }

    private void RefreshTemplates()
    {
        Templates = new(TemplateStorage.Instance?.Templates.Where(_ => _.Categories.Contains(SelectedCategory) && Landscape == _.Landscape));
    }
}