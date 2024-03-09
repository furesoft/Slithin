using System.Windows.Input;
using AuroraModularis.Core;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.Templates.UI.Commands;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Templates.UI.ViewModels;

internal class TemplatesPageViewModel : BaseViewModel, IFilterable<TemplatesFilter>
{
    private readonly ILoadingService _loadingService;
    private readonly ITemplateStorage _templateStorage;

    public TemplatesPageViewModel(TemplatesFilter templatesFilter,
        ITemplateStorage templateStorage,
        IDialogService dialogService,
        ILoadingService loadingService)
    {
        Filter = templatesFilter;
        _templateStorage = templateStorage;
        _loadingService = loadingService;

        OpenAddModalCommand = new DelegateCommand(async _ =>
        {
            var vm = ServiceContainer.Current.Resolve<AddTemplateModalViewModel>();
            if (await dialogService.Show("", new AddTemplateModal {DataContext = vm}))
            {
                vm.AcceptCommand.Execute(vm);
            }
        });

        RemoveTemplateCommand = ServiceContainer.Current.Resolve<RemoveTemplateCommand>();
    }

    public ICommand OpenAddModalCommand { get; set; }

    public ICommand RemoveTemplateCommand { get; set; }

    public TemplatesFilter Filter { get; }

    protected override async void OnLoad()
    {
        Filter.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName != nameof(Filter.Items))
            {
                Filter.Items = new(_templateStorage.Templates!.Where(_ =>
                    _.Categories.Contains(Filter.SelectedCategory)
                    && Filter.Landscape == _.Landscape));
            }
        };

        await _loadingService.LoadTemplatesAsync();

        Filter.SelectedCategory = Filter.Categories.FirstOrDefault();
    }
}
