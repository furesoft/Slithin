using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Notebooks.UI.ViewModels;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.UI;

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
        if (_lb.SelectedItem is not FileSystemModel fsm || fsm is not DirectoryModel ||
            _lb.DataContext is not NotebooksPageViewModel vm)
        {
            return;
        }

        var notebooksFilter = ServiceContainer.Current.Resolve<NotebooksFilter>();
        var mdStorage = ServiceContainer.Current.Resolve<IMetadataRepository>();

        notebooksFilter.Items.Clear();

        var id = fsm.ID;

        notebooksFilter.ParentFolder = (DirectoryModel)fsm;

        if (!string.IsNullOrEmpty(fsm.VisibleName) && fsm is UpDirectoryModel upFsm)
        {
            id = _lastFolderIDs.Pop();
            notebooksFilter.Folder = id;
            notebooksFilter.ParentFolder = (DirectoryModel)upFsm.ParentFolder;

            if (id == "")
            {
                notebooksFilter.Items.Add(new TrashModel());
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
                notebooksFilter.Items.Add(new DirectoryModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
            }
            else
            {
                notebooksFilter.Items.Add(new FileModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
            }
        }

        if (_lastFolderIDs.Count > 0)
        {
            notebooksFilter.Items.Add(new UpDirectoryModel(fsm));
            notebooksFilter.Folder = id;
        }
        else
        {
            notebooksFilter.Folder = "";
        }

        notebooksFilter.SortByFolder();
    }
}
