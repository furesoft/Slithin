using System.Windows.Input;
using AuroraModularis.Core;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Templates.UI.Commands;

[Context(UIContext.Templates)]
internal class RemoveTemplateCommand : ICommand, IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly TemplatesFilter _templatesFilter;
    private readonly ITemplateStorage _templateStorage;
    private readonly IDialogService _dialogService;

    public RemoveTemplateCommand(ILocalisationService localisationService,
                                 TemplatesFilter templatesFilter,
                                 ITemplateStorage templateStorage,
                                 IDialogService dialogService)
    {
        _localisationService = localisationService;
        _templatesFilter = templatesFilter;
        _templateStorage = templateStorage;
        _dialogService = dialogService;
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }

    public TranslatedString Title => "Remove";

    public bool CanExecute(object parameter)
    {
        return parameter is Template;
    }

    public async void Execute(object parameter)
    {
        if (parameter is not Template tmpl
            || !await _dialogService.Show(
                _localisationService.GetStringFormat("Would you really want to delete '{0}'?", tmpl.Filename)))
        {
            return;
        }

        ServiceContainer.Current.Resolve<TemplatesFilter>().Selection = null;
        _templatesFilter.Items.Remove(tmpl);

        _templateStorage.Remove(tmpl);
        //_localRepository.RemoveTemplate(tmpl);

        //_deviceRepository.RemoveTemplate(tmpl);
    }
}
