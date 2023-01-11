using System.Collections.ObjectModel;
using Slithin.Core.MVVM;

using Slithin.Modules.Notebooks.UI.Models;

namespace Slithin.Modules.Sync.Models;

public class NotebooksFilter : NotifyObject
{
    private ObservableCollection<FileSystemModel> _documents = new();
    private string _folder = "";

    private FileSystemModel _selectedNotebook;

    public ObservableCollection<FileSystemModel> Documents
    {
        get => _documents;
        set => SetValue(ref _documents, value);
    }

    public string Folder
    {
        get => _folder;
        set => SetValue(ref _folder, value);
    }

    public FileSystemModel SelectedNotebook
    {
        get => _selectedNotebook;
        set => SetValue(ref _selectedNotebook, value);
    }

    public void SortByFolder()
    {
        var ordered = Documents.OrderByDescending(_ => _.IsPinned);
        ordered = ordered.OrderByDescending(_ => _ is DirectoryModel);
        ordered = ordered.OrderByDescending(_ => _.VisibleName?.Equals("Up .."));

        Documents = new ObservableCollection<FileSystemModel>(ordered);
    }
}
