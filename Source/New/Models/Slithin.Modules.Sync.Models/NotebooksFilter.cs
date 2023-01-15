using System.Collections.ObjectModel;
using Slithin.Core;

using Slithin.Modules.Notebooks.UI.Models;

namespace Slithin.Modules.Sync.Models;

public class NotebooksFilter : FilterBase<FileSystemModel>
{
    private string _folder = "";

    public string Folder
    {
        get => _folder;
        set => SetValue(ref _folder, value);
    }

    public void SortByFolder()
    {
        var ordered = Items.OrderByDescending(_ => _.IsPinned);
        ordered = ordered.OrderByDescending(_ => _ is UpDirectoryModel);
        ordered = ordered.OrderByDescending(_ => _ is DirectoryModel);

        Items = new ObservableCollection<FileSystemModel>(ordered);
    }
}
