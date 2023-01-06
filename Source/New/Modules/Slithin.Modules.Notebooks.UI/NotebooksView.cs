using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Notebooks.UI.ViewModels;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI;

public static class NotebooksView
{
    private static readonly Stack<string> _lastFolderIDs = new();
    private static ListBox _lb;

    public static bool GetIsView(Control control)
    {
        return control == _lb;
    }

    public static void SetIsView(ListBox lb, bool value)
    {
        _lb = lb;

        _lb.DoubleTapped += _lb_DoubleTapped;
    }

    private static void _lb_DoubleTapped(object sender, RoutedEventArgs e)
    {
        if (_lb.SelectedItem is not FilesystemModel fsm || fsm is not DirectoryModel ||
            _lb.DataContext is not NotebooksPageViewModel vm)
        {
            return;
        }

        var notebooksFilter = Container.Current.Resolve<NotebooksFilter>();
        var mdStorage = Container.Current.Resolve<IMetadataRepository>();

        notebooksFilter.Documents.Clear();

        var id = fsm.ID;

        if (!string.IsNullOrEmpty(fsm.VisibleName) && fsm is UpDirectoryModel)
        {
            id = _lastFolderIDs.Pop();
            notebooksFilter.Folder = id;

            if (id == "")
            {
                notebooksFilter.Documents.Add(new TrashModel());
            }
        }
        else if (fsm is TrashModel)
        {
            id = "trash";
            _lastFolderIDs.Push("");
        }
        else
        {
            _lastFolderIDs.Push(fsm.Parent);
        }

        vm.IsInTrash = id == "trash";

        foreach (var mds in mdStorage.GetByParent(id))
        {
            if (mds.Type == "CollectionType")
            {
                notebooksFilter.Documents.Add(new DirectoryModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
            }
            else
            {
                notebooksFilter.Documents.Add(new FileModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
            }
        }

        if (_lastFolderIDs.Count > 0)
        {
            notebooksFilter.Documents.Add(new UpDirectoryModel());
            notebooksFilter.Folder = id;
        }
        else
        {
            notebooksFilter.Folder = "";
        }

        notebooksFilter.SortByFolder();
    }
}
