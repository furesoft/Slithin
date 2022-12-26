using System.Collections.ObjectModel;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Sync.Models;

public class NotebooksFilter : NotifyObject
{
    private ObservableCollection<Metadata> _documents = new();
    private string _folder = "";

    private Metadata _selectedNotebook;

    public ObservableCollection<Metadata> Documents
    {
        get => _documents;
        set => SetValue(ref _documents, value);
    }

    public string Folder
    {
        get => _folder;
        set => SetValue(ref _folder, value);
    }

    public Metadata SelectedNotebook
    {
        get => _selectedNotebook;
        set => SetValue(ref _selectedNotebook, value);
    }

    public void SortByFolder()
    {
        var ordered = Documents.OrderByDescending(_ => _.IsPinned);
        ordered = ordered.OrderByDescending(_ => _.Type == "CollectionType");
        ordered = ordered.OrderByDescending(_ => _.VisibleName?.Equals("Up .."));

        Documents = new ObservableCollection<Metadata>(ordered);
    }
}
