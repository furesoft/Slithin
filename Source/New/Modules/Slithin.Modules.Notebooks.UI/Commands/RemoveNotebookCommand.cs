using System.Windows.Input;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Notebooks.UI.Commands;

[Context(UIContext.Notebook)]
public class RemoveNotebookCommand : ICommand
{
    private readonly ILocalisationService _localisationService;

    public RemoveNotebookCommand(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
    }

    public event EventHandler CanExecuteChanged;

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Remove");

    public bool CanExecute(object parameter)
    {
        return parameter != null
               && parameter is Metadata md
               && md.VisibleName != _localisationService.GetString("Quick sheets")
               && md.VisibleName != _localisationService.GetString("Up ..")
               && md.VisibleName != _localisationService.GetString("Trash");
    }

    public async void Execute(object parameter)
    {
        /*
        if (parameter is not Metadata md
            || !await DialogService.ShowDialog(
                _localisationService.GetStringFormat("Would you really want to delete '{0}'?", md.VisibleName)))
            return;

        ServiceLocator.Container.Resolve<NotebooksPageViewModel>().SelectedNotebook = null;

        MetadataStorage.Local.Remove(md);
        _localRepository.Remove(md);
        _deviceRepository.Remove(md);

        _synchronisationService.NotebooksFilter.Documents.Clear();

        foreach (var mds in MetadataStorage.Local.GetByParent(md.Parent))
        {
            ServiceLocator.SyncService.NotebooksFilter.Documents.Add(mds);
        }
        if (md.Parent != "")
        {
            ServiceLocator.SyncService.NotebooksFilter.Documents.Add(
                new Metadata
                {
                    Type = "CollectionType",
                    VisibleName = _localisationService.GetString("Up ..")
                });
        }

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();
        */
    }
}
