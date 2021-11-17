using System;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.ViewModels.Pages;

namespace Slithin.Core.Commands;

public class RemoveTemplateCommand : ICommand
{
    private readonly LocalRepository _localRepository;
    private readonly SynchronisationService _synchronisationService;
    private readonly TemplatesPageViewModel _templatesPageViewModel;

    public RemoveTemplateCommand(TemplatesPageViewModel templatesPageViewModel,
        LocalRepository localRepository)
    {
        _templatesPageViewModel = templatesPageViewModel;
        _localRepository = localRepository;
        _synchronisationService = ServiceLocator.SyncService;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return parameter is Template;
    }

    public async void Execute(object parameter)
    {
        if (parameter is not Template tmpl
            || !await DialogService.ShowDialog($"Would you really want to delete '{tmpl.Filename}'?"))
            return;

        _templatesPageViewModel.SelectedTemplate = null;
        _synchronisationService.TemplateFilter.Templates.Remove(tmpl);

        TemplateStorage.Instance.Remove(tmpl);
        _localRepository.Remove(tmpl);

        var item = new SyncItem
        {
            Action = SyncAction.Remove,
            Direction = SyncDirection.ToDevice,
            Data = tmpl,
            Type = SyncType.Template
        };

        _synchronisationService.AddToSyncQueue(item);
    }
}