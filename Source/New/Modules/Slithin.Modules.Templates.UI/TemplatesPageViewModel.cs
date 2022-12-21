using System.Windows.Input;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Templates.UI;

internal class TemplatesPageViewModel : BaseViewModel
{
    private readonly ITemplateStorage _templateStorage;
    private readonly ILoadingService _loadingService;
    private Template _selectedTemplate;

    public TemplatesPageViewModel(IPathManager pathManager,
                                  ILocalisationService localisationService,
                                  TemplatesFilter templatesFilter,
                                  ITemplateStorage templateStorage,
                                  ILoadingService loadingService)
    {
        TemplateFilter = templatesFilter;
        _templateStorage = templateStorage;
        _loadingService = loadingService;
        /*
OpenAddModalCommand = new DelegateCommand(_ =>
{
DialogService.Open(new AddTemplateModal(),
new AddTemplateModalViewModel(pathManager, localRepository, localisationService, validator));
});

RemoveTemplateCommand = new RemoveTemplateCommand(this, localisationService,
deviceRepository, localRepository);
*/
    }

    public ICommand OpenAddModalCommand { get; set; }

    public ICommand RemoveTemplateCommand { get; set; }

    public Template SelectedTemplate
    {
        get => _selectedTemplate;
        set => SetValue(ref _selectedTemplate, value);
    }

    public TemplatesFilter TemplateFilter { get; }

    public override void OnLoad()
    {
        base.OnLoad();

        TemplateFilter.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName != nameof(TemplateFilter.Templates))
            {
                TemplateFilter.Templates = new(_templateStorage.Templates.Where(_ => _.Categories.Contains(TemplateFilter.SelectedCategory) && TemplateFilter.Landscape == _.Landscape));
            }
        };

        _loadingService.LoadTemplates();

        TemplateFilter.SelectedCategory = TemplateFilter.Categories.First();
    }
}
