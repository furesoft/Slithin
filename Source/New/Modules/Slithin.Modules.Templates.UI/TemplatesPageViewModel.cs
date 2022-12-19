using System.Windows.Input;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Templates.UI;

public class TemplatesPageViewModel : BaseViewModel
{
    private Template _selectedTemplate;

    public TemplatesPageViewModel(IPathManager pathManager,
        ILocalisationService localisationService)
    {
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

    public override void OnLoad()
    {
        base.OnLoad();
    }
}
