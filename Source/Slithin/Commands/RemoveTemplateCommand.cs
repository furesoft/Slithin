using System;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.ViewModels.Pages;

namespace Slithin.Commands;

[Context(UIContext.Template)]
public class RemoveTemplateCommand : ICommand, IContextCommand
{
    private readonly DeviceRepository _deviceRepository;
    private readonly ILocalisationService _localisationService;
    private readonly LocalRepository _localRepository;
    private readonly SynchronisationService _synchronisationService;
    private readonly TemplatesPageViewModel _templatesPageViewModel;

    public RemoveTemplateCommand(TemplatesPageViewModel templatesPageViewModel,
                                 ILocalisationService localisationService,
                                 DeviceRepository deviceRepository,
                                 LocalRepository localRepository)
    {
        _templatesPageViewModel = templatesPageViewModel;
        _localisationService = localisationService;
        _deviceRepository = deviceRepository;
        _localRepository = localRepository;
        _synchronisationService = ServiceLocator.SyncService;
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }

    public string Titel => _localisationService.GetString("Remove");

    public bool CanExecute(object parameter)
    {
        return parameter is Template;
    }

    public bool CanHandle(object data)
    {
        return CanExecute(data);
    }

    public async void Execute(object parameter)
    {
        if (parameter is not Template tmpl
            || !await DialogService.ShowDialog(
                _localisationService.GetStringFormat("Would you really want to delete '{0}'?", tmpl.Filename)))
        {
            return;
        }

        _templatesPageViewModel.SelectedTemplate = null;
        _synchronisationService.TemplateFilter.Templates.Remove(tmpl);

        TemplateStorage.Instance.Remove(tmpl);
        _localRepository.RemoveTemplate(tmpl);

        _deviceRepository.RemoveTemplate(tmpl);
    }

    public void Invoke(object data)
    {
        Execute(data);
    }
}
